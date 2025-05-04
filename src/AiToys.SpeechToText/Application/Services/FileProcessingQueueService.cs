using System.Collections.Concurrent;
using AiToys.SpeechToText.Application.UseCases;
using AiToys.SpeechToText.Domain.Enums;
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
    private readonly IFileStatusNotifierService fileStatusNotifierService;
    private readonly ILogger<FileProcessingQueueService> logger;
    private readonly BlockingCollection<FileItemQueueModel> fileQueue = [];
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly Task processingTask;
    private bool isDisposed;

    public FileProcessingQueueService(
        ITranscribeFileUseCase transcribeFileUseCase,
        IFileStatusNotifierService fileStatusNotifierService,
        ILogger<FileProcessingQueueService> logger
    )
    {
        this.transcribeFileUseCase = transcribeFileUseCase;
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
            file.SetStatus(FileItemStatus.Processing);

            fileStatusNotifierService.NotifyStatusChanged(file.FilePath, FileItemStatus.Processing);

            await transcribeFileUseCase
                .ExecuteAsync(
                    file.FilePath,
                    queueItem.SourceLanguage,
                    queueItem.TargetLanguage,
                    cancellationTokenSource.Token
                )
                .ConfigureAwait(false);

            if (file.Status == FileItemStatus.Processing)
            {
                file.SetStatus(FileItemStatus.Completed);

                fileStatusNotifierService.NotifyStatusChanged(file.FilePath, FileItemStatus.Completed);

                logger.LogInformation("File processing completed: {FilePath}", file.FilePath);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("File processing was canceled: {FilePath}", file.FilePath);

            if (file.Status == FileItemStatus.Processing)
            {
                file.SetStatus(FileItemStatus.Added);

                fileStatusNotifierService.NotifyStatusChanged(file.FilePath, FileItemStatus.Added);
            }
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Invalid operation error while processing file: {FilePath}", file.FilePath);

            file.SetStatus(FileItemStatus.Failed);

            fileStatusNotifierService.NotifyStatusChanged(file.FilePath, FileItemStatus.Failed);
        }
    }

    private record FileItemQueueModel(FileItemModel FileItem, string SourceLanguage, string? TargetLanguage);
}
