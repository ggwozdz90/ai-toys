using AiToys.SpeechToText.Domain.Models;

namespace AiToys.SpeechToText.Domain.Repositories;

internal interface ISpeechToTextRepository
{
    Task<IReadOnlyList<LanguageModel>> GetSupportedLanguagesAsync(CancellationToken cancellationToken = default);

    Task<string> TranscribeFileAsync(
        string filePath,
        string sourceLanguage,
        CancellationToken cancellationToken = default
    );

    Task<string> TranscribeAndTranslateFileAsync(
        string filePath,
        string sourceLanguage,
        string targetLanguage,
        CancellationToken cancellationToken = default
    );
}
