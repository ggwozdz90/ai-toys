using System.Collections.ObjectModel;
using System.ComponentModel;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.ViewModels;
using AiToys.SpeechToText.Application.UseCases;
using AiToys.SpeechToText.Constants;
using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Presentation.Factories;
using Extensions.Hosting.WinUi;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Presentation.ViewModels;

internal sealed partial class SpeechToTextViewModel : ViewModelBase, IRouteAwareViewModel, IInitializableViewModel
{
    private readonly ILogger<SpeechToTextViewModel> logger;
    private readonly IGetSupportedLanguagesUseCase getSupportedLanguagesUseCase;
    private readonly IGetDefaultFileExtensionsUseCase getDefaultFileExtensionsUseCase;
    private readonly ISelectFilesUseCase selectFilesUseCase;
    private readonly ISelectFolderUseCase selectFolderUseCase;
    private readonly IHealthCheckUseCase healthCheckUseCase;
    private bool isInitialized;
    private LanguageModel? defaultSourceLanguage;
    private LanguageModel? defaultTargetLanguage;
    private bool generateBothTranscriptionAndTranslation;
    private string fileExtensions = string.Empty;
    private bool isApiHealthy;

    public SpeechToTextViewModel(
        IDispatcherService dispatcherService,
        ILogger<SpeechToTextViewModel> logger,
        ILoggerFactory loggerFactory,
        IFileItemViewModelFactory fileItemViewModelFactory,
        IGetSupportedLanguagesUseCase getSupportedLanguagesUseCase,
        IGetDefaultFileExtensionsUseCase getDefaultFileExtensionsUseCase,
        ISelectFilesUseCase selectFilesUseCase,
        ISelectFolderUseCase selectFolderUseCase,
        IHealthCheckUseCase healthCheckUseCase
    )
        : base(dispatcherService)
    {
        this.logger = logger;
        this.getSupportedLanguagesUseCase = getSupportedLanguagesUseCase;
        this.getDefaultFileExtensionsUseCase = getDefaultFileExtensionsUseCase;
        this.selectFilesUseCase = selectFilesUseCase;
        this.selectFolderUseCase = selectFolderUseCase;
        this.healthCheckUseCase = healthCheckUseCase;

        FileQueueViewModel = new FileQueueViewModel(
            dispatcherService,
            loggerFactory.CreateLogger<FileQueueViewModel>(),
            fileItemViewModelFactory
        );

        FileQueueViewModel.PropertyChanged += OnFileQueuePropertyChanged;

        InitializeCommand = new AsyncRelayCommand(ExecuteInitializeAsync);
        BrowseFileCommand = new AsyncRelayCommand(ExecuteBrowseFileAsync);
        BrowseFolderCommand = new AsyncRelayCommand(ExecuteBrowseFolderAsync);
        SwapLanguagesCommand = new RelayCommand(ExecuteSwapLanguages);
        ApplyDefaultLanguagesCommand = new RelayCommand(ExecuteApplyDefaultLanguages, CanExecuteApplyDefaultLanguages);
        CheckApiHealthCommand = new AsyncRelayCommand(ExecuteCheckApiHealthAsync);
    }

    public string Route => RouteNames.SpeechToTextPage;

    public ICommandBase InitializeCommand { get; }
    public ICommandBase BrowseFileCommand { get; }
    public ICommandBase BrowseFolderCommand { get; }
    public ICommandBase SwapLanguagesCommand { get; }
    public ICommandBase ApplyDefaultLanguagesCommand { get; }
    public ICommandBase CheckApiHealthCommand { get; }
    public ObservableCollection<LanguageModel> Languages { get; } = [];
    public FileQueueViewModel FileQueueViewModel { get; }

    public LanguageModel? DefaultSourceLanguage
    {
        get => defaultSourceLanguage;
        set
        {
            if (SetProperty(ref defaultSourceLanguage, value) && value != null)
            {
                FileQueueViewModel.SourceLanguage = value;
            }
        }
    }

    public LanguageModel? DefaultTargetLanguage
    {
        get => defaultTargetLanguage;
        set
        {
            if (SetProperty(ref defaultTargetLanguage, value) && value != null)
            {
                FileQueueViewModel.TargetLanguage = value;
            }
        }
    }

    public bool GenerateBothTranscriptionAndTranslation
    {
        get => generateBothTranscriptionAndTranslation;
        set
        {
            if (SetProperty(ref generateBothTranscriptionAndTranslation, value))
            {
                FileQueueViewModel.GenerateBothTranscriptionAndTranslation = value;
            }
        }
    }

    public string FileExtensions
    {
        get => fileExtensions;
        set => SetProperty(ref fileExtensions, value);
    }

    public bool IsApiHealthy
    {
        get => isApiHealthy;
        private set => SetProperty(ref isApiHealthy, value);
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
            CheckApiHealthCommand.Dispose();
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

    private async Task ExecuteCheckApiHealthAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking API health");

            IsApiHealthy = await healthCheckUseCase.ExecuteAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation(
                "API health check completed. API is {HealthStatus}",
                IsApiHealthy ? "healthy" : "unhealthy"
            );
        }
        catch (HealthCheckException ex)
        {
            logger.LogError(ex, "Error checking API health: {ErrorMessage}", ex.Message);
            IsApiHealthy = false;
        }
    }

    private async Task ExecuteInitializeAsync(CancellationToken cancellationToken)
    {
        if (isInitialized)
        {
            return;
        }

        try
        {
            logger.LogInformation("Initializing SpeechToTextViewModel");

            var supportedLanguages = await getSupportedLanguagesUseCase
                .ExecuteAsync(cancellationToken)
                .ConfigureAwait(false);

            var defaultFileExtensions = await getDefaultFileExtensionsUseCase
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

            FileExtensions = defaultFileExtensions;
            logger.LogInformation("Default file extensions loaded: {Extensions}", FileExtensions);

            await ExecuteCheckApiHealthAsync(cancellationToken).ConfigureAwait(false);

            isInitialized = true;

            logger.LogInformation("SpeechToTextViewModel initialized successfully");
        }
        catch (GetSupportedLanguagesException ex)
        {
            logger.LogError(ex, "Error retrieving supported languages: {ErrorMessage}", ex.Message);
        }
        catch (GetDefaultFileExtensionsException ex)
        {
            logger.LogError(ex, "Error retrieving default file extensions: {ErrorMessage}", ex.Message);
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

            var fileItems = await selectFilesUseCase
                .ExecuteAsync(FileExtensions, cancellationToken)
                .ConfigureAwait(false);

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

            var fileItems = await selectFolderUseCase
                .ExecuteAsync(FileExtensions, cancellationToken)
                .ConfigureAwait(false);

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
