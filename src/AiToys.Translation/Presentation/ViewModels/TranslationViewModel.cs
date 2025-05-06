using System.Collections.ObjectModel;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.ViewModels;
using AiToys.Translation.Application.UseCases;
using AiToys.Translation.Constants;
using AiToys.Translation.Domain.Exceptions;
using AiToys.Translation.Domain.Models;
using Extensions.Hosting.WinUi;
using Microsoft.Extensions.Logging;

namespace AiToys.Translation.Presentation.ViewModels;

internal sealed partial class TranslationViewModel : ViewModelBase, IRouteAwareViewModel, IInitializableViewModel
{
    private readonly ITranslateTextUseCase translateTextUseCase;
    private readonly IGetSupportedLanguagesUseCase getSupportedLanguagesUseCase;
    private readonly IHealthCheckUseCase healthCheckUseCase;
    private readonly ILogger<TranslationViewModel> logger;

    private CancellationTokenSource? currentTranslationCts;
    private LanguageModel? selectedSourceLanguage;
    private LanguageModel? selectedTargetLanguage;
    private string sourceText = string.Empty;
    private string translatedText = string.Empty;
    private bool isTranslating;
    private bool isInitialized;
    private bool isApiHealthy;

    public TranslationViewModel(
        IDispatcherService dispatcherService,
        ITranslateTextUseCase translateTextUseCase,
        IGetSupportedLanguagesUseCase getSupportedLanguagesUseCase,
        IHealthCheckUseCase healthCheckUseCase,
        ILogger<TranslationViewModel> logger
    )
        : base(dispatcherService)
    {
        this.translateTextUseCase = translateTextUseCase;
        this.getSupportedLanguagesUseCase = getSupportedLanguagesUseCase;
        this.healthCheckUseCase = healthCheckUseCase;
        this.logger = logger;

        var translateCommand = new AsyncRelayCommand(ExecuteTranslateAsync, CanExecuteTranslate);
        translateCommand
            .ObservesProperty(this, nameof(IsTranslating))
            .ObservesProperty(this, nameof(SourceText))
            .ObservesProperty(this, nameof(SelectedSourceLanguage))
            .ObservesProperty(this, nameof(SelectedTargetLanguage));

        TranslateCommand = translateCommand;

        var swapLanguagesCommand = new RelayCommand(ExecuteSwapLanguages, CanExecuteSwapLanguages);
        swapLanguagesCommand
            .ObservesProperty(this, nameof(IsTranslating))
            .ObservesProperty(this, nameof(SelectedSourceLanguage))
            .ObservesProperty(this, nameof(SelectedTargetLanguage));
        SwapLanguagesCommand = swapLanguagesCommand;

        InitializeCommand = new AsyncRelayCommand(ExecuteInitializeAsync);

        CheckApiHealthCommand = new AsyncRelayCommand(ExecuteCheckApiHealthAsync);
    }

    public string Route => RouteNames.TranslationPage;

    public ObservableCollection<LanguageModel> Languages { get; } = [];

    public LanguageModel? SelectedSourceLanguage
    {
        get => selectedSourceLanguage;
        set => SetProperty(ref selectedSourceLanguage, value);
    }

    public LanguageModel? SelectedTargetLanguage
    {
        get => selectedTargetLanguage;
        set => SetProperty(ref selectedTargetLanguage, value);
    }

    public string SourceText
    {
        get => sourceText;
        set => SetProperty(ref sourceText, value);
    }

    public string TranslatedText
    {
        get => translatedText;
        set => SetProperty(ref translatedText, value);
    }

    public bool IsTranslating
    {
        get => isTranslating;
        set => SetProperty(ref isTranslating, value);
    }

    public bool IsApiHealthy
    {
        get => isApiHealthy;
        private set => SetProperty(ref isApiHealthy, value);
    }

    public ICommandBase TranslateCommand { get; }

    public ICommandBase SwapLanguagesCommand { get; }

    public ICommandBase InitializeCommand { get; }

    public ICommandBase CheckApiHealthCommand { get; }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TranslateCommand.Dispose();
            SwapLanguagesCommand.Dispose();
            InitializeCommand.Dispose();
            CheckApiHealthCommand.Dispose();

            CancelCurrentTranslation();
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
                SelectedSourceLanguage = Languages[0];
                SelectedTargetLanguage = Languages.Count > 1 ? Languages[1] : Languages[0];
            }

            await ExecuteCheckApiHealthAsync(cancellationToken).ConfigureAwait(false);

            isInitialized = true;

            logger.LogInformation("TranslationViewModel initialized successfully");
        }
        catch (GetSupportedLanguagesException ex)
        {
            logger.LogError(ex, "Error retrieving supported languages: {ErrorMessage}", ex.Message);
        }
    }

    private async Task ExecuteTranslateAsync(CancellationToken cancellationToken)
    {
        CancelCurrentTranslation();

        if (string.IsNullOrWhiteSpace(SourceText) || SelectedSourceLanguage == null || SelectedTargetLanguage == null)
        {
            return;
        }

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        currentTranslationCts = linkedCts;

        try
        {
            IsTranslating = true;

            logger.LogInformation(
                "Translating text from {SourceLanguageModel} to {TargetLanguageModel}",
                SelectedSourceLanguage.Code,
                SelectedTargetLanguage.Code
            );

            var result = await translateTextUseCase
                .ExecuteAsync(SourceText, SelectedSourceLanguage.Code, SelectedTargetLanguage.Code, linkedCts.Token)
                .ConfigureAwait(false);

            if (!linkedCts.IsCancellationRequested)
            {
                TranslatedText = result;
                logger.LogInformation("Translation completed successfully");
            }
        }
        catch (TranslateTextException ex)
        {
            logger.LogError(ex, "Error during translation: {ErrorMessage}", ex.Message);
            TranslatedText = "An error occurred during translation. Please try again later.";
        }
        finally
        {
            if (currentTranslationCts == linkedCts)
            {
                currentTranslationCts = null;
            }

            linkedCts.Dispose();
            IsTranslating = false;
        }
    }

    private bool CanExecuteTranslate()
    {
        return !IsTranslating
            && !string.IsNullOrWhiteSpace(SourceText)
            && SelectedSourceLanguage != null
            && SelectedTargetLanguage != null;
    }

    private void CancelCurrentTranslation()
    {
        if (currentTranslationCts == null)
        {
            return;
        }

        if (!currentTranslationCts.IsCancellationRequested)
        {
            currentTranslationCts.Cancel();
        }

        currentTranslationCts.Dispose();
        currentTranslationCts = null;
    }

    private void ExecuteSwapLanguages()
    {
        if (SelectedSourceLanguage == null || SelectedTargetLanguage == null)
        {
            return;
        }

        logger.LogInformation(
            "Swapping languages from {SourceLanguageModel} to {TargetLanguageModel}",
            SelectedSourceLanguage.Code,
            SelectedTargetLanguage.Code
        );

        (SelectedSourceLanguage, SelectedTargetLanguage) = (SelectedTargetLanguage, SelectedSourceLanguage);

        SourceText = TranslatedText;
        TranslatedText = string.Empty;
    }

    private bool CanExecuteSwapLanguages()
    {
        return !IsTranslating && SelectedSourceLanguage != null && SelectedTargetLanguage != null;
    }

    private async Task ExecuteCheckApiHealthAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking translation API health");

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
}
