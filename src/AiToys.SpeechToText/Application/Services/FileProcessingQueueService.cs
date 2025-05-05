using System.Collections.Concurrent;
using AiToys.SpeechToText.Application.UseCases;
using AiToys.SpeechToText.Domain.Enums;
using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Models;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Application.Services;

internal interface IFileProcessingQueueService
{
    void EnqueueFile(FileItemModel file, string sourceLanguage, string? targetLanguage = null);

    void RemoveFile(FileItemModel file);
}

internal sealed partial class FileProcessingQueueService : IFileProcessingQueueService, IDisposable
{
    private readonly ITranscribeFileUseCase transcribeFileUseCase;
    private readonly ISaveTranscriptionUseCase saveTranscriptionUseCase;
    private readonly IFileStatusNotifierService fileStatusNotifierService;
    private readonly ILogger<FileProcessingQueueService> logger;
    private readonly BlockingCollection<FileItemQueueModel> fileQueue = [];
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly Task processingTask;
    private bool isDisposed;

    public FileProcessingQueueService(
        ITranscribeFileUseCase transcribeFileUseCase,
        ISaveTranscriptionUseCase saveTranscriptionUseCase,
        IFileStatusNotifierService fileStatusNotifierService,
        ILogger<FileProcessingQueueService> logger
    )
    {
        this.transcribeFileUseCase = transcribeFileUseCase;
        this.saveTranscriptionUseCase = saveTranscriptionUseCase;
        this.fileStatusNotifierService = fileStatusNotifierService;
        this.logger = logger;

        cancellationTokenSource = new CancellationTokenSource();
        processingTask = Task.Run(ProcessQueueAsync, cancellationTokenSource.Token);
    }

    public void EnqueueFile(FileItemModel file, string sourceLanguage, string? targetLanguage = null)
    {
        logger.LogInformation("Enqueuing file: {FilePath}", file.FilePath);

        var queueItem = new FileItemQueueModel(file, sourceLanguage, targetLanguage);

        file.SetStatus(FileItemStatus.Pending);

        fileStatusNotifierService.NotifyStatusChanged(file.FilePath, FileItemStatus.Pending);

        fileQueue.Add(queueItem, cancellationTokenSource.Token);

        logger.LogInformation("File enqueued: {FilePath}", file.FilePath);
    }

    public void RemoveFile(FileItemModel file)
    {
        logger.LogInformation("Removing file: {FilePath}", file.FilePath);

        if (file.Status is FileItemStatus.Pending)
        {
            file.SetStatus(FileItemStatus.Added);

            fileStatusNotifierService.NotifyStatusChanged(file.FilePath, FileItemStatus.Added);
        }

        logger.LogInformation("File removed: {FilePath}", file.FilePath);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }

    private void Dispose(bool disposing)
    {
        if (isDisposed)
        {
            return;
        }

        if (disposing)
        {
            cancellationTokenSource.Cancel();

            try
            {
                processingTask.Wait(TimeSpan.FromSeconds(5), CancellationToken.None);
            }
            catch (AggregateException ex) when (ex.InnerExceptions.Any(e => e is TaskCanceledException))
            {
                logger.LogDebug("Processing task was canceled as expected during disposal");
            }
            catch (TaskCanceledException)
            {
                logger.LogDebug("Processing task was canceled during disposal");
            }

            cancellationTokenSource.Dispose();
            fileQueue.Dispose();
        }

        isDisposed = true;
    }

    private async Task ProcessQueueAsync()
    {
        logger.LogInformation("Starting file processing queue worker");

        try
        {
            foreach (var item in fileQueue.GetConsumingEnumerable(cancellationTokenSource.Token))
            {
                await ProcessFileAsync(item).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("File processing queue worker was canceled");
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Queue operation failed in file processing queue worker");
        }

        logger.LogInformation("File processing queue worker stopped");
    }

    private async Task ProcessFileAsync(FileItemQueueModel queueItem)
    {
        var file = queueItem.FileItem;

        logger.LogInformation("Processing file: {FilePath}", file.FilePath);

        if (file.Status != FileItemStatus.Pending)
        {
            logger.LogInformation(
                "Skipping file processing as status is not Pending: {FilePath}, Status: {Status}",
                file.FilePath,
                file.Status
            );

            return;
        }

        try
        {
            await ProcessPendingFileAsync(queueItem).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            HandleCanceledProcessing(file);
        }
        catch (InvalidOperationException ex)
        {
            HandleProcessingError(file, ex, "Invalid operation error while processing file");
        }
        catch (SaveTranscriptionException ex)
        {
            HandleProcessingError(file, ex, "Error saving transcription for file");
        }
        catch (TranscribeFileException ex)
        {
            HandleProcessingError(file, ex, "Error transcribing file");
        }
    }

    private async Task ProcessPendingFileAsync(FileItemQueueModel queueItem)
    {
        var file = queueItem.FileItem;

        UpdateFileStatus(file, FileItemStatus.Processing);

        var transcription = await transcribeFileUseCase
            .ExecuteAsync(
                queueItem.FileItem.FilePath,
                queueItem.SourceLanguage,
                queueItem.TargetLanguage,
                cancellationTokenSource.Token
            )
            .ConfigureAwait(false);

        var language =
            queueItem.TargetLanguage != null
            && !string.Equals(queueItem.SourceLanguage, queueItem.TargetLanguage, StringComparison.Ordinal)
                ? queueItem.TargetLanguage
                : queueItem.SourceLanguage;

        await saveTranscriptionUseCase
            .ExecuteAsync(queueItem.FileItem.FilePath, transcription, language, cancellationTokenSource.Token)
            .ConfigureAwait(false);

        file.SetTranscription(transcription);

        if (file.Status == FileItemStatus.Processing)
        {
            UpdateFileStatus(file, FileItemStatus.Completed);

            logger.LogInformation("File processing completed: {FilePath}", file.FilePath);
        }
    }

    private void HandleCanceledProcessing(FileItemModel file)
    {
        logger.LogInformation("File processing was canceled: {FilePath}", file.FilePath);

        if (file.Status == FileItemStatus.Processing)
        {
            UpdateFileStatus(file, FileItemStatus.Added);
        }
    }

    private void HandleProcessingError(FileItemModel file, Exception ex, string errorMessage)
    {
        logger.LogError(ex, "{ErrorMessage}: {FilePath}", errorMessage, file.FilePath);

        UpdateFileStatus(file, FileItemStatus.Failed);
    }

    private void UpdateFileStatus(FileItemModel file, FileItemStatus status)
    {
        file.SetStatus(status);

        fileStatusNotifierService.NotifyStatusChanged(file.FilePath, status);
    }

    private record FileItemQueueModel(FileItemModel FileItem, string SourceLanguage, string? TargetLanguage);
}
