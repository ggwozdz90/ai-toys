using System.Collections.ObjectModel;
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

        InitializeCommand = new AsyncRelayCommand(ExecuteInitializeAsync);
        BrowseFileCommand = new AsyncRelayCommand(ExecuteBrowseFileAsync);
        BrowseFolderCommand = new AsyncRelayCommand(ExecuteBrowseFolderAsync);
    }

    public string Route => RouteNames.SpeechToTextPage;

    public ICommandBase InitializeCommand { get; }
    public ICommandBase BrowseFileCommand { get; }
    public ICommandBase BrowseFolderCommand { get; }
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
            InitializeCommand.Dispose();
            BrowseFileCommand.Dispose();
            BrowseFolderCommand.Dispose();
            FileQueueViewModel.Dispose();
        }

        base.Dispose(disposing);
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
            }

            if (Languages.Count > 0)
            {
                DefaultSourceLanguage = Languages[0];
                DefaultTargetLanguage = Languages.Count > 1 ? Languages[1] : Languages[0];
            }

            isInitialized = true;

            logger.LogInformation("TranslationViewModel initialized successfully");
        }
        catch (GetSupportedLanguagesException ex)
        {
            logger.LogError(ex, "Error retrieving supported languages: {ErrorMessage}", ex.Message);
        }
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
}
