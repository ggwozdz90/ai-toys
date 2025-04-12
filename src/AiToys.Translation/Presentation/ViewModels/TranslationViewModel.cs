using System.Collections.ObjectModel;
using System.Windows.Input;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.ViewModels;
using AiToys.Translation.Application.UseCases;
using AiToys.Translation.Constants;
using AiToys.Translation.Domain.Exceptions;
using AiToys.Translation.Domain.Models;
using Microsoft.Extensions.Logging;

namespace AiToys.Translation.Presentation.ViewModels;

internal sealed partial class TranslationViewModel
    : ViewModelBase,
        IRouteAwareViewModel,
        IInitializableViewModel,
        IDisposable
{
    private readonly ITranslateTextUseCase translateTextUseCase;
    private readonly IGetSupportedLanguagesUseCase getSupportedLanguagesUseCase;
    private readonly ILogger<TranslationViewModel> logger;

    private CancellationTokenSource? currentTranslationCts;
    private bool disposed;
    private LanguageModel? selectedSourceLanguage;
    private LanguageModel? selectedTargetLanguage;
    private string sourceText = string.Empty;
    private string translatedText = string.Empty;
    private bool isTranslating;
    private bool isInitialized;

    public TranslationViewModel(
        ITranslateTextUseCase translateTextUseCase,
        IGetSupportedLanguagesUseCase getSupportedLanguagesUseCase,
        ILogger<TranslationViewModel> logger
    )
    {
        this.translateTextUseCase = translateTextUseCase;
        this.getSupportedLanguagesUseCase = getSupportedLanguagesUseCase;
        this.logger = logger;

        TranslateCommand = new AsyncRelayCommand(ExecuteTranslateAsync, CanExecuteTranslate);
        SwapLanguagesCommand = new RelayCommand(ExecuteSwapLanguages, CanExecuteSwapLanguages);
        InitializeCommand = new AsyncRelayCommand(ExecuteInitializeAsync);
    }

    public string Route => RouteNames.TranslationPage;

    public ObservableCollection<LanguageModel> Languages { get; } = [];

    public LanguageModel? SelectedSourceLanguage
    {
        get => selectedSourceLanguage;
        set
        {
            if (SetProperty(ref selectedSourceLanguage, value))
            {
                logger.LogInformation("Source language changed to {LanguageModel}", value?.Code);
                UpdateCommandsCanExecute();
            }
        }
    }

    public LanguageModel? SelectedTargetLanguage
    {
        get => selectedTargetLanguage;
        set
        {
            if (SetProperty(ref selectedTargetLanguage, value))
            {
                logger.LogInformation("Target language changed to {LanguageModel}", value?.Code);
                UpdateCommandsCanExecute();
            }
        }
    }

    public string SourceText
    {
        get => sourceText;
        set
        {
            if (SetProperty(ref sourceText, value))
            {
                UpdateCommandsCanExecute();
            }
        }
    }

    public string TranslatedText
    {
        get => translatedText;
        set => SetProperty(ref translatedText, value);
    }

    public bool IsTranslating
    {
        get => isTranslating;
        set
        {
            if (SetProperty(ref isTranslating, value))
            {
                UpdateCommandsCanExecute();
            }
        }
    }

    public ICommand TranslateCommand { get; }

    public ICommand SwapLanguagesCommand { get; }

    public ICommand InitializeCommand { get; }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        (TranslateCommand as IDisposable)?.Dispose();
        (SwapLanguagesCommand as IDisposable)?.Dispose();
        (InitializeCommand as IDisposable)?.Dispose();

        CancelCurrentTranslation();

        disposed = true;
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

    private void UpdateCommandsCanExecute()
    {
        (TranslateCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
        (SwapLanguagesCommand as RelayCommand)?.NotifyCanExecuteChanged();
    }
}
