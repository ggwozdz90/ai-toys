using AiToys.Translation.Domain.Models;

namespace AiToys.Translation.Domain.Repositories;

internal interface ITranslationRepository
{
    Task<IReadOnlyList<LanguageModel>> GetSupportedLanguagesAsync(CancellationToken cancellationToken = default);

    Task<string> TranslateTextAsync(
        string sourceText,
        string sourceLanguageCode,
        string targetLanguageCode,
        CancellationToken cancellationToken = default
    );

    Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default);
}
