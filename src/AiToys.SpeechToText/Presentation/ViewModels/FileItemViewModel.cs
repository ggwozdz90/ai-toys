using System.Collections.ObjectModel;
using System.Windows.Input;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.ViewModels;
using AiToys.SpeechToText.Application.UseCases;
using AiToys.SpeechToText.Domain.Enums;
using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Presentation.EventArgs;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Presentation.ViewModels;

internal sealed partial class FileItemViewModel : ViewModelBase
{
    private readonly ILogger<FileItemViewModel> logger;
    private readonly ITranscribeFileUseCase transcribeFileUseCase;
    private readonly FileItemModel fileModel;
    private FileItemStatus status;
    private LanguageModel sourceLanguage;
    private LanguageModel targetLanguage;
    private CancellationTokenSource? currentProcessingCts;

    public FileItemViewModel(
        ILogger<FileItemViewModel> logger,
        ITranscribeFileUseCase transcribeFileUseCase,
        FileItemModel fileModel,
        LanguageModel sourceLanguage,
        LanguageModel targetLanguage,
        ObservableCollection<LanguageModel> availableLanguages
    )
    {
        this.logger = logger;
        this.transcribeFileUseCase = transcribeFileUseCase;
        this.fileModel = fileModel;
        this.sourceLanguage = sourceLanguage;
        this.targetLanguage = targetLanguage;

        AvailableLanguages = availableLanguages;

        status = fileModel.Status;

        var startProcessingCommand = new AsyncRelayCommand(ExecuteStartProcessingAsync, CanExecuteStartProcessing);
        startProcessingCommand.ObservesProperty(this, nameof(Status));
        StartProcessingCommand = startProcessingCommand;

        var stopProcessingCommand = new AsyncRelayCommand(ExecuteStopProcessingAsync, CanExecuteStopProcessing);
        stopProcessingCommand.ObservesProperty(this, nameof(Status));
        StopProcessingCommand = stopProcessingCommand;

        RemoveCommand = new RelayCommand(ExecuteRemoveCommand);
    }

    public event EventHandler<FileItemEventArgs>? RemoveRequested;

    public string FilePath => fileModel.FilePath;
    public string FileName => fileModel.FileName;
    public string Transcription => fileModel.Transcription;

    public FileItemStatus Status
    {
        get => status;
        set
        {
            if (SetProperty(ref status, value))
            {
                fileModel.SetStatus(value);
            }
        }
    }

    public LanguageModel SourceLanguage
    {
        get => sourceLanguage;
        set => SetProperty(ref sourceLanguage, value);
    }

    public LanguageModel TargetLanguage
    {
        get => targetLanguage;
        set => SetProperty(ref targetLanguage, value);
    }

    public ObservableCollection<LanguageModel> AvailableLanguages { get; }

    public ICommand StartProcessingCommand { get; }
    public ICommand StopProcessingCommand { get; }
    public ICommand RemoveCommand { get; }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            CancelCurrentProcessing();

            (StartProcessingCommand as IDisposable)?.Dispose();
            (StopProcessingCommand as IDisposable)?.Dispose();
            (RemoveCommand as IDisposable)?.Dispose();
        }

        base.Dispose(disposing);
    }

    private static string GetExceptionLogMessage(Exception exception)
    {
        return exception switch
        {
            ArgumentException => "Invalid argument for file",
            FileNotFoundException => "File not found",
            IOException => "I/O error for file",
            UnauthorizedAccessException => "Access denied for file",
            TranscribeFileException => "Error processing file",
            _ => "Error processing file",
        };
    }

    private static string GetUserFriendlyErrorMessage(Exception exception)
    {
        return exception switch
        {
            ArgumentException => $"Invalid input: {exception.Message}",
            FileNotFoundException => $"File not found: {exception.Message}",
            IOException => $"I/O error: {exception.Message}",
            UnauthorizedAccessException => $"Access denied: {exception.Message}",
            TranscribeFileException => $"Error: {exception.Message}",
            _ => $"Error: {exception.Message}",
        };
    }

    private async Task ExecuteStartProcessingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting processing of file: {FilePath}", FilePath);

        CancelCurrentProcessing();

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        currentProcessingCts = linkedCts;

        try
        {
            Status = FileItemStatus.Processing;

            var targetLanguageCode = GetTargetLanguageCode();
            var result = await ProcessTranscriptionAsync(targetLanguageCode, linkedCts.Token).ConfigureAwait(true);

            if (!linkedCts.IsCancellationRequested)
            {
                fileModel.SetTranscription(result);
                Status = FileItemStatus.Completed;

                logger.LogInformation("Successfully processed file: {FilePath}", FilePath);
            }
        }
        catch (OperationCanceledException ex)
        {
            logger.LogInformation(ex, "Processing of file {FilePath} was canceled", FilePath);
            Status = FileItemStatus.Pending;
        }
        catch (Exception ex)
            when (ex
                    is ArgumentException
                        or FileNotFoundException
                        or IOException
                        or UnauthorizedAccessException
                        or TranscribeFileException
            )
        {
            HandleProcessingException(
                ex,
                GetExceptionLogMessage(ex),
                FileItemStatus.Failed,
                GetUserFriendlyErrorMessage(ex)
            );
        }
        finally
        {
            if (currentProcessingCts == linkedCts)
            {
                currentProcessingCts = null;
            }

            linkedCts.Dispose();
        }
    }

    private string? GetTargetLanguageCode()
    {
        return !string.Equals(sourceLanguage.Code, targetLanguage.Code, StringComparison.Ordinal)
            ? targetLanguage.Code
            : null;
    }

    private Task<string> ProcessTranscriptionAsync(string? targetLanguageCode, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Transcribing file: {FilePath} from {SourceLanguage}{TargetLanguage}",
            FilePath,
            sourceLanguage.Code,
            targetLanguageCode == null ? string.Empty : $" to {targetLanguageCode}"
        );

        return transcribeFileUseCase.ExecuteAsync(FilePath, sourceLanguage.Code, targetLanguageCode, cancellationToken);
    }

    private void HandleProcessingException(
        Exception exception,
        string logMessage,
        FileItemStatus newStatus,
        string resultMessage
    )
    {
        logger.LogError(exception, "{Message} {FilePath}: {ErrorMessage}", logMessage, FilePath, exception.Message);
        Status = newStatus;
        fileModel.SetTranscription(resultMessage);
    }

    private bool CanExecuteStartProcessing() => Status is FileItemStatus.Pending or FileItemStatus.Failed;

    private Task ExecuteStopProcessingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping processing of file: {FilePath}", FilePath);

        CancelCurrentProcessing();

        Status = FileItemStatus.Pending;

        return Task.CompletedTask;
    }

    private bool CanExecuteStopProcessing() => Status == FileItemStatus.Processing;

    private void ExecuteRemoveCommand()
    {
        logger.LogInformation("Removing file from queue: {FilePath}", FilePath);

        CancelCurrentProcessing();

        RemoveRequested?.Invoke(this, new FileItemEventArgs(this));
    }

    private void CancelCurrentProcessing()
    {
        if (currentProcessingCts == null)
        {
            return;
        }

        if (!currentProcessingCts.IsCancellationRequested)
        {
            currentProcessingCts.Cancel();
        }

        currentProcessingCts.Dispose();
        currentProcessingCts = null;
    }
}
