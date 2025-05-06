using AiToys.Translation.Domain.Models;
using AiToys.Translation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using TranslationApiClient.Adapter.Adapters;

namespace AiToys.Translation.Data.Repositories;

internal sealed class TranslationRepository(
    ITranslationAdapter translationAdapter,
    ILogger<TranslationRepository> logger
) : ITranslationRepository
{
    private readonly IReadOnlyList<LanguageModel> supportedLanguages =
    [
        new("en_US", "English (US)"),
        new("pl_PL", "Polish"),
        new("de_DE", "German"),
        new("fr_FR", "French"),
        new("es_ES", "Spanish"),
        new("it_IT", "Italian"),
        new("pt_PT", "Portuguese"),
        new("ru_RU", "Russian"),
        new("ja_JP", "Japanese"),
        new("zh_CN", "Chinese (Simplified)"),
    ];

    public Task<IReadOnlyList<LanguageModel>> GetSupportedLanguagesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting supported languages");

        return Task.FromResult(supportedLanguages);
    }

    public async Task<string> TranslateTextAsync(
        string sourceText,
        string sourceLanguageCode,
        string targetLanguageCode,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation(
            "Translating text from {SourceLanguageCode} to {TargetLanguageCode}",
            sourceLanguageCode,
            targetLanguageCode
        );

        try
        {
            var translatedText = await translationAdapter
                .TranslateAsync(sourceText, sourceLanguageCode, targetLanguageCode)
                .ConfigureAwait(false);

            logger.LogInformation(
                "Translation completed successfully from {SourceLanguageCode} to {TargetLanguageCode}",
                sourceLanguageCode,
                targetLanguageCode
            );

            return translatedText;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error occurred while translating text from {SourceLanguageCode} to {TargetLanguageCode}: {ErrorMessage}",
                sourceLanguageCode,
                targetLanguageCode,
                ex.Message
            );

            throw;
        }
    }

    public async Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Checking API health status");

        var result = await translationAdapter.HealthCheckAsync().ConfigureAwait(false);
        var isHealthy = string.Equals(result, "OK", StringComparison.Ordinal);

        logger.LogInformation("API health check result: {Result}, IsHealthy: {IsHealthy}", result, isHealthy);

        return isHealthy;
    }
}
