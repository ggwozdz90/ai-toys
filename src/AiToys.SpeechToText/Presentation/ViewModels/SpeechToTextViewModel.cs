using System.Collections.ObjectModel;
using System.ComponentModel;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.ViewModels;
using AiToys.SpeechToText.Application.UseCases;
using AiToys.SpeechToText.Constants;
using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Presentation.Factories;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Presentation.ViewModels;

internal sealed partial class SpeechToTextViewModel : ViewModelBase, IRouteAwareViewModel, IInitializableViewModel
{
    private readonly ILogger<SpeechToTextViewModel> logger;
    private readonly IGetSupportedLanguagesUseCase getSupportedLanguagesUseCase;
    private readonly ISelectFilesUseCase selectFilesUseCase;
    private readonly ISelectFolderUseCase selectFolderUseCase;
    private bool isInitialized;
    private LanguageModel? defaultSourceLanguage;
    private LanguageModel? defaultTargetLanguage;

    public SpeechToTextViewModel(
        ILogger<SpeechToTextViewModel> logger,
        ILoggerFactory loggerFactory,
        IFileItemViewModelFactory fileItemViewModelFactory,
        IGetSupportedLanguagesUseCase getSupportedLanguagesUseCase,
        ISelectFilesUseCase selectFilesUseCase,
        ISelectFolderUseCase selectFolderUseCase
    )
    {
        this.logger = logger;
        this.getSupportedLanguagesUseCase = getSupportedLanguagesUseCase;
        this.selectFilesUseCase = selectFilesUseCase;
        this.selectFolderUseCase = selectFolderUseCase;

        FileQueueViewModel = new FileQueueViewModel(
            loggerFactory.CreateLogger<FileQueueViewModel>(),
            fileItemViewModelFactory
        );

        FileQueueViewModel.PropertyChanged += OnFileQueuePropertyChanged;

        InitializeCommand = new AsyncRelayCommand(ExecuteInitializeAsync);
        BrowseFileCommand = new AsyncRelayCommand(ExecuteBrowseFileAsync);
        BrowseFolderCommand = new AsyncRelayCommand(ExecuteBrowseFolderAsync);
        SwapLanguagesCommand = new RelayCommand(ExecuteSwapLanguages);
        ApplyDefaultLanguagesCommand = new RelayCommand(ExecuteApplyDefaultLanguages, CanExecuteApplyDefaultLanguages);
    }

    public string Route => RouteNames.SpeechToTextPage;

    public ICommandBase InitializeCommand { get; }
    public ICommandBase BrowseFileCommand { get; }
    public ICommandBase BrowseFolderCommand { get; }
    public ICommandBase SwapLanguagesCommand { get; }
    public ICommandBase ApplyDefaultLanguagesCommand { get; }
    public ObservableCollection<LanguageModel> Languages { get; } = [];
    public FileQueueViewModel FileQueueViewModel { get; }

    public LanguageModel? DefaultSourceLanguage
    {
        get => defaultSourceLanguage;
        set => SetProperty(ref defaultSourceLanguage, value);
    }

    public LanguageModel? DefaultTargetLanguage
    {
        get => defaultTargetLanguage;
        set => SetProperty(ref defaultTargetLanguage, value);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            FileQueueViewModel.PropertyChanged -= OnFileQueuePropertyChanged;

            InitializeCommand.Dispose();
            BrowseFileCommand.Dispose();
            BrowseFolderCommand.Dispose();
            SwapLanguagesCommand.Dispose();
            ApplyDefaultLanguagesCommand.Dispose();
            FileQueueViewModel.Dispose();
        }

        base.Dispose(disposing);
    }

    private bool CanExecuteApplyDefaultLanguages() => FileQueueViewModel.Files.Any();

    private void ExecuteApplyDefaultLanguages()
    {
        logger.LogInformation("Applying default languages to all files");

        FileQueueViewModel.ApplyLanguagesCommand.Execute(parameter: null);

        logger.LogInformation("Default languages applied to all files");
    }

    private async Task ExecuteInitializeAsync(CancellationToken cancellationToken)
    {
        if (isInitialized)
        {
            return;
        }

        try
        {
            logger.LogInformation("Initializing TranslationViewModel");

            var supportedLanguages = await getSupportedLanguagesUseCase
                .ExecuteAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var language in supportedLanguages)
            {
                Languages.Add(language);
                FileQueueViewModel.AvailableLanguages.Add(language);
            }

            if (Languages.Count > 0)
            {
                DefaultSourceLanguage = Languages[0];
                DefaultTargetLanguage = Languages.Count > 1 ? Languages[1] : Languages[0];

                FileQueueViewModel.SourceLanguage = DefaultSourceLanguage;
                FileQueueViewModel.TargetLanguage = DefaultTargetLanguage;
            }

            isInitialized = true;

            logger.LogInformation("TranslationViewModel initialized successfully");
        }
        catch (GetSupportedLanguagesException ex)
        {
            logger.LogError(ex, "Error retrieving supported languages: {ErrorMessage}", ex.Message);
        }
    }

    private void ExecuteSwapLanguages()
    {
        if (DefaultSourceLanguage == null || DefaultTargetLanguage == null)
        {
            return;
        }

        logger.LogInformation(
            "Swapping languages from {SourceLanguage} to {TargetLanguage}",
            DefaultSourceLanguage.Code,
            DefaultTargetLanguage.Code
        );

        (DefaultSourceLanguage, DefaultTargetLanguage) = (DefaultTargetLanguage, DefaultSourceLanguage);

        FileQueueViewModel.SourceLanguage = DefaultSourceLanguage;
        FileQueueViewModel.TargetLanguage = DefaultTargetLanguage;
    }

    private async Task ExecuteBrowseFileAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Executing browse file command");

            var fileItems = await selectFilesUseCase.ExecuteAsync(cancellationToken).ConfigureAwait(false);

            if (fileItems.Count > 0)
            {
                logger.LogInformation("Selected {Count} files", fileItems.Count);

                FileQueueViewModel.AddFiles(fileItems);
            }
            else
            {
                logger.LogInformation("File selection canceled or no files selected");
            }
        }
        catch (SelectFilesException ex)
        {
            logger.LogError(ex, "Error browsing for files: {ErrorMessage}", ex.Message);
        }
    }

    private async Task ExecuteBrowseFolderAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Executing browse folder command");

            var fileItems = await selectFolderUseCase.ExecuteAsync(cancellationToken).ConfigureAwait(false);

            if (fileItems.Count > 0)
            {
                logger.LogInformation("Selected {Count} files", fileItems.Count);

                FileQueueViewModel.AddFiles(fileItems);
            }
            else
            {
                logger.LogInformation("Folder selection canceled or no files selected");
            }
        }
        catch (SelectFolderException ex)
        {
            logger.LogError(ex, "Error browsing for folder: {ErrorMessage}", ex.Message);
        }
    }

    private void OnFileQueuePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (string.Equals(e.PropertyName, nameof(FileQueueViewModel.HasFiles), StringComparison.Ordinal))
        {
            ApplyDefaultLanguagesCommand.NotifyCanExecuteChanged();
        }
    }
}
