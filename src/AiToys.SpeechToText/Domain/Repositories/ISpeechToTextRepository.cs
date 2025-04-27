using AiToys.SpeechToText.Domain.Models;

namespace AiToys.SpeechToText.Domain.Repositories;

internal interface ISpeechToTextRepository
{
    Task<IReadOnlyList<LanguageModel>> GetSupportedLanguagesAsync(CancellationToken cancellationToken = default);
}
