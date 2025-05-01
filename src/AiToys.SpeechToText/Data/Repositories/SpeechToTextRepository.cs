using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Domain.Repositories;
using Microsoft.Extensions.Logging;
using SpeechToTextApiClient.Adapter.Adapters;

namespace AiToys.SpeechToText.Data.Repositories;

internal sealed class SpeechToTextRepository(
    ILogger<SpeechToTextRepository> logger,
    ISpeechToTextAdapter speechToTextAdapter
) : ISpeechToTextRepository
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

    public async Task<string> TranscribeFileAsync(
        string filePath,
        string sourceLanguage,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation("Transcribing file: {FilePath} from {SourceLanguage}", filePath, sourceLanguage);

        var result = await speechToTextAdapter
            .TranscribeAsync(filePath: filePath, sourceLanguage: sourceLanguage)
            .ConfigureAwait(false);

        logger.LogInformation("Successfully transcribed file: {FilePath}", filePath);

        return result;
    }

    public async Task<string> TranscribeAndTranslateFileAsync(
        string filePath,
        string sourceLanguage,
        string targetLanguage,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation(
            "Transcribing and translating file: {FilePath} from {SourceLanguage} to {TargetLanguage}",
            filePath,
            sourceLanguage,
            targetLanguage
        );

        var result = await speechToTextAdapter
            .TranscribeAndTranslateAsync(
                filePath: filePath,
                sourceLanguage: sourceLanguage,
                targetLanguage: targetLanguage
            )
            .ConfigureAwait(false);

        logger.LogInformation(
            "Successfully transcribed and translated file: {FilePath} from {SourceLanguage} to {TargetLanguage}",
            filePath,
            sourceLanguage,
            targetLanguage
        );

        return result;
    }
}
