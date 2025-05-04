using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.ViewModels;
using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Presentation.EventArgs;
using AiToys.SpeechToText.Presentation.Factories;
using Extensions.Hosting.WinUi;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Presentation.ViewModels;

internal sealed partial class FileQueueViewModel : ViewModelBase
{
    private readonly ILogger<FileQueueViewModel> logger;
    private readonly IFileItemViewModelFactory fileItemViewModelFactory;
    private bool hasFiles;

    public FileQueueViewModel(
        IDispatcherService dispatcherService,
        ILogger<FileQueueViewModel> logger,
        IFileItemViewModelFactory fileItemViewModelFactory
    )
        : base(dispatcherService)
    {
        this.logger = logger;
        this.fileItemViewModelFactory = fileItemViewModelFactory;

        Files.CollectionChanged += Files_CollectionChanged;

        StartAllCommand = new RelayCommand(ExecuteStartAll, CanExecuteStartAll);
        StopAllCommand = new RelayCommand(ExecuteStopAll, CanExecuteStopAll);
        ClearAllCommand = new RelayCommand(ExecuteClearAll, CanExecuteClearAll);
        ApplyLanguagesCommand = new RelayCommand(ExecuteApplyLanguages, CanExecuteApplyLanguages);
    }

    public ObservableCollection<FileItemViewModel> Files { get; } = [];

    public bool HasFiles
    {
        get => hasFiles;
        private set => SetProperty(ref hasFiles, value);
    }

    public LanguageModel? SourceLanguage { get; set; }
    public LanguageModel? TargetLanguage { get; set; }
    public ObservableCollection<LanguageModel> AvailableLanguages { get; } = [];

    public ICommandBase StartAllCommand { get; }
    public ICommandBase StopAllCommand { get; }
    public ICommandBase ClearAllCommand { get; }
    public ICommandBase ApplyLanguagesCommand { get; }

    public void AddFile(FileItemModel fileItem)
    {
        if (SourceLanguage == null || TargetLanguage == null)
        {
            logger.LogWarning("Source or target language is not set. Cannot add file to queue.");

            return;
        }

        logger.LogInformation("Adding file to queue: {FilePath}", fileItem.FilePath);

        var fileItemViewModel = fileItemViewModelFactory.Create(
            fileItem,
            SourceLanguage,
            TargetLanguage,
            AvailableLanguages
        );
        fileItemViewModel.RemoveRequested += OnFileRemoveRequested;
        fileItemViewModel.PropertyChanged += OnFileItemPropertyChanged;

        ExecuteOnUIThread(() => Files.Add(fileItemViewModel));
    }

    public void AddFiles(IEnumerable<FileItemModel> fileItems)
    {
        var fileItemsToAdd = fileItems.ToList();

        if (fileItemsToAdd.Count == 0)
        {
            return;
        }

        foreach (var fileItem in fileItemsToAdd)
        {
            AddFile(fileItem);
        }

        logger.LogInformation("Added {Count} files to the queue", fileItemsToAdd.Count);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Files.CollectionChanged -= Files_CollectionChanged;

            foreach (var file in Files)
            {
                file.RemoveRequested -= OnFileRemoveRequested;
                file.PropertyChanged -= OnFileItemPropertyChanged;
                file.Dispose();
            }

            ExecuteOnUIThread(() => Files.Clear());

            StartAllCommand.Dispose();
            StopAllCommand.Dispose();
            ClearAllCommand.Dispose();
            ApplyLanguagesCommand.Dispose();

            logger.LogInformation("FileQueueViewModel is being disposed");
        }

        base.Dispose(disposing);
    }

    private void ExecuteApplyLanguages()
    {
        if (SourceLanguage == null || TargetLanguage == null)
        {
            logger.LogWarning("Source or target language is not set. Cannot apply languages to files.");

            return;
        }

        logger.LogInformation("Applying default languages to all files in queue");

        foreach (var file in Files)
        {
            file.SourceLanguage = SourceLanguage;
            file.TargetLanguage = TargetLanguage;
        }

        logger.LogInformation("Default languages applied to all files");
    }

    private bool CanExecuteApplyLanguages() => Files.Any();

    private void OnFileItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (string.Equals(e.PropertyName, nameof(FileItemViewModel.Status), StringComparison.Ordinal))
        {
            RaiseCommandsCanExecuteChanged();
        }
    }

    private void RaiseCommandsCanExecuteChanged()
    {
        ExecuteOnUIThread(() =>
        {
            StartAllCommand.NotifyCanExecuteChanged();
            StopAllCommand.NotifyCanExecuteChanged();
            ClearAllCommand.NotifyCanExecuteChanged();
            ApplyLanguagesCommand.NotifyCanExecuteChanged();
        });
    }

    private void ExecuteStartAll()
    {
        logger.LogInformation("Starting processing of all files in queue");

        foreach (var file in Files)
        {
            if (file.StartProcessingCommand.CanExecute(parameter: null))
            {
                file.StartProcessingCommand.Execute(parameter: null);
            }
        }

        logger.LogInformation("All files started processing");
    }

    private bool CanExecuteStartAll() => Files.Any(file => file.StartProcessingCommand.CanExecute(parameter: null));

    private void ExecuteStopAll()
    {
        logger.LogInformation("Stopping processing of all files in queue");

        foreach (var file in Files)
        {
            if (file.StopProcessingCommand.CanExecute(parameter: null))
            {
                file.StopProcessingCommand.Execute(parameter: null);
            }
        }

        logger.LogInformation("All files stopped processing");
    }

    private bool CanExecuteStopAll() => Files.Any(file => file.StopProcessingCommand.CanExecute(parameter: null));

    private void ExecuteClearAll()
    {
        logger.LogInformation("Clearing all files from queue");

        foreach (var file in Files.ToList())
        {
            file.RemoveRequested -= OnFileRemoveRequested;
            file.PropertyChanged -= OnFileItemPropertyChanged;
        }

        ExecuteOnUIThread(() => Files.Clear());

        logger.LogInformation("All files cleared from queue");
    }

    private bool CanExecuteClearAll() => Files.Any();

    private void Files_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateHasFiles();
        RaiseCommandsCanExecuteChanged();
    }

    private void UpdateHasFiles()
    {
        HasFiles = Files.Count > 0;
    }

    private void OnFileRemoveRequested(object? sender, FileItemEventArgs eventArgs)
    {
        var fileItem = eventArgs.FileItem;

        if (Files.Contains(fileItem))
        {
            logger.LogInformation("Removing file from queue: {FilePath}", fileItem.FilePath);

            fileItem.RemoveRequested -= OnFileRemoveRequested;
            fileItem.PropertyChanged -= OnFileItemPropertyChanged;

            ExecuteOnUIThread(() => Files.Remove(fileItem));
        }
    }
}
