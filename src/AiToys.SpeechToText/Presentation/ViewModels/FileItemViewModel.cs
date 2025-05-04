using System.Collections.ObjectModel;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.ViewModels;
using AiToys.SpeechToText.Application.Services;
using AiToys.SpeechToText.Domain.Enums;
using AiToys.SpeechToText.Domain.Events;
using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Presentation.EventArgs;
using Extensions.Hosting.WinUi;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Presentation.ViewModels;

internal sealed partial class FileItemViewModel : ViewModelBase
{
    private readonly ILogger<FileItemViewModel> logger;
    private readonly IFileProcessingQueueService fileProcessingQueueService;
    private readonly IFileStatusNotifierService fileStatusNotifierService;
    private readonly FileItemModel fileModel;
    private FileItemStatus status;
    private string errorMessage = string.Empty;
    private LanguageModel sourceLanguage;
    private LanguageModel targetLanguage;

    public FileItemViewModel(
        IDispatcherService dispatcherService,
        ILogger<FileItemViewModel> logger,
        IFileProcessingQueueService fileProcessingQueueService,
        IFileStatusNotifierService fileStatusNotifierService,
        FileItemModel fileModel,
        LanguageModel sourceLanguage,
        LanguageModel targetLanguage,
        ObservableCollection<LanguageModel> availableLanguages
    )
        : base(dispatcherService)
    {
        this.logger = logger;
        this.fileProcessingQueueService = fileProcessingQueueService;
        this.fileStatusNotifierService = fileStatusNotifierService;
        this.fileModel = fileModel;
        this.sourceLanguage = sourceLanguage;
        this.targetLanguage = targetLanguage;

        AvailableLanguages = availableLanguages;

        status = fileModel.Status;

        this.fileStatusNotifierService.FileStatusChanged += OnFileStatusChanged;

        var startProcessingCommand = new RelayCommand(ExecuteStartProcessing, CanExecuteStartProcessing);
        startProcessingCommand.ObservesProperty(this, nameof(Status));
        StartProcessingCommand = startProcessingCommand;

        var stopProcessingCommand = new RelayCommand(ExecuteStopProcessing, CanExecuteStopProcessing);
        stopProcessingCommand.ObservesProperty(this, nameof(Status));
        StopProcessingCommand = stopProcessingCommand;

        RemoveCommand = new RelayCommand(ExecuteRemoveCommand, CanExecuteRemoveCommand);
    }

    public event EventHandler<FileItemEventArgs>? RemoveRequested;

    public string FilePath => fileModel.FilePath;
    public string FileName => fileModel.FileName;

    public string ErrorMessage
    {
        get => errorMessage;
        set => SetProperty(ref errorMessage, value);
    }

    public FileItemStatus Status
    {
        get => status;
        set => SetProperty(ref status, value);
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

    public ICommandBase StartProcessingCommand { get; }
    public ICommandBase StopProcessingCommand { get; }
    public ICommandBase RemoveCommand { get; }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            fileStatusNotifierService.FileStatusChanged -= OnFileStatusChanged;

            StartProcessingCommand.Dispose();
            StopProcessingCommand.Dispose();
            RemoveCommand.Dispose();
        }

        base.Dispose(disposing);
    }

    private void ExecuteStartProcessing()
    {
        logger.LogInformation("Adding file to processing queue: {FilePath}", FilePath);

        try
        {
            fileProcessingQueueService.EnqueueFile(
                fileModel,
                sourceLanguage.Code,
                !string.Equals(sourceLanguage.Code, targetLanguage.Code, StringComparison.Ordinal)
                    ? targetLanguage.Code
                    : null
            );

            logger.LogInformation("File successfully added to processing queue: {FilePath}", FilePath);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Invalid argument error adding file to processing queue: { FilePath}", FilePath);
            ErrorMessage = $"Error: {ex.Message}";

            fileModel.SetStatus(FileItemStatus.Failed);
        }
    }

    private bool CanExecuteStartProcessing() => Status is FileItemStatus.Added or FileItemStatus.Failed;

    private void ExecuteStopProcessing()
    {
        logger.LogInformation("Removing file from processing queue: {FilePath}", FilePath);

        try
        {
            fileProcessingQueueService.RemoveFile(fileModel);

            logger.LogInformation("File successfully removed from processing queue: {FilePath}", FilePath);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Invalid argument error removing file from processing queue: {FilePath}", FilePath);
            ErrorMessage = $"Error: {ex.Message}";

            fileModel.SetStatus(FileItemStatus.Failed);
        }
    }

    private bool CanExecuteStopProcessing() => Status is FileItemStatus.Pending;

    private void ExecuteRemoveCommand()
    {
        logger.LogInformation("Removing file from view: {FilePath}", FilePath);

        fileProcessingQueueService.RemoveFile(fileModel);

        RemoveRequested?.Invoke(this, new FileItemEventArgs(this));
    }

    private bool CanExecuteRemoveCommand() => Status is not FileItemStatus.Processing;

    private void OnFileStatusChanged(object? sender, FileStatusChangedEventArgs e)
    {
        if (!string.Equals(e.FilePath, FilePath, StringComparison.Ordinal))
        {
            return;
        }

        logger.LogDebug("File status changed: {FilePath}, {Status}", FilePath, e.Status);

        Status = e.Status;

        ExecuteOnUIThread(() =>
        {
            StartProcessingCommand.NotifyCanExecuteChanged();
            StopProcessingCommand.NotifyCanExecuteChanged();
            RemoveCommand.NotifyCanExecuteChanged();
        });
    }
}
