using System.Collections.ObjectModel;
using System.Windows.Input;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.ViewModels;
using AiToys.SpeechToText.Domain.Enums;
using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Presentation.EventArgs;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Presentation.ViewModels;

internal sealed partial class FileItemViewModel : ViewModelBase
{
    private readonly ILogger<FileItemViewModel> logger;
    private readonly FileItemModel fileModel;
    private FileItemStatus status;
    private LanguageModel sourceLanguage;
    private LanguageModel targetLanguage;

    public FileItemViewModel(
        ILogger<FileItemViewModel> logger,
        FileItemModel fileModel,
        LanguageModel sourceLanguage,
        LanguageModel targetLanguage,
        ObservableCollection<LanguageModel> availableLanguages
    )
    {
        this.fileModel = fileModel;
        this.logger = logger;
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
            (StartProcessingCommand as IDisposable)?.Dispose();
            (StopProcessingCommand as IDisposable)?.Dispose();
            (RemoveCommand as IDisposable)?.Dispose();
        }

        base.Dispose(disposing);
    }

    private Task ExecuteStartProcessingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting processing of file: {FilePath}", FilePath);

        Status = FileItemStatus.Processing;

        return Task.Delay(2000, cancellationToken);
    }

    private bool CanExecuteStartProcessing() => Status is FileItemStatus.Pending or FileItemStatus.Failed;

    private Task ExecuteStopProcessingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping processing of file: {FilePath}", FilePath);

        Status = FileItemStatus.Pending;

        return Task.Delay(2000, cancellationToken);
    }

    private bool CanExecuteStopProcessing() => Status == FileItemStatus.Processing;

    private void ExecuteRemoveCommand()
    {
        logger.LogInformation("Removing file from queue: {FilePath}", FilePath);

        RemoveRequested?.Invoke(this, new FileItemEventArgs(this));
    }
}
