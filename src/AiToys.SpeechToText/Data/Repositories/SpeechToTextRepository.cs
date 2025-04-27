using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Data.Repositories;

internal sealed class SpeechToTextRepository(ILogger<SpeechToTextRepository> logger) : ISpeechToTextRepository
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
}
