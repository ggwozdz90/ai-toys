using AiToys.Translation.Domain.Exceptions;
using AiToys.Translation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiToys.Translation.Application.UseCases;

internal interface ITranslateTextUseCase
{
    Task<string> ExecuteAsync(
        string sourceText,
        string sourceLanguageCode,
        string targetLanguageCode,
        CancellationToken cancellationToken = default
    );
}

internal sealed class TranslateTextUseCase(
    ITranslationRepository translationRepository,
    ILogger<TranslateTextUseCase> logger
) : ITranslateTextUseCase
{
    public async Task<string> ExecuteAsync(
        string sourceText,
        string sourceLanguageCode,
        string targetLanguageCode,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(sourceText))
        {
            logger.LogWarning("Source text is empty or null");
            return string.Empty;
        }

        if (string.IsNullOrWhiteSpace(sourceLanguageCode))
        {
            throw new ArgumentException("Source language code cannot be null or empty", nameof(sourceLanguageCode));
        }

        if (string.IsNullOrWhiteSpace(targetLanguageCode))
        {
            throw new ArgumentException("Target language code cannot be null or empty", nameof(targetLanguageCode));
        }

        if (string.Equals(sourceLanguageCode, targetLanguageCode, StringComparison.Ordinal))
        {
            logger.LogInformation("Source and target languages are the same, returning original text");
            return sourceText;
        }

        logger.LogInformation(
            "Translating text from {SourceLanguageCode} to {TargetLanguageCode}",
            sourceLanguageCode,
            targetLanguageCode
        );

        try
        {
            var translatedText = await translationRepository
                .TranslateTextAsync(sourceText, sourceLanguageCode, targetLanguageCode, cancellationToken)
                .ConfigureAwait(false);

            logger.LogInformation(
                "Successfully translated text from {SourceLanguageCode} to {TargetLanguageCode}",
                sourceLanguageCode,
                targetLanguageCode
            );

            return translatedText;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error translating text from {SourceLanguageCode} to {TargetLanguageCode}: {ErrorMessage}",
                sourceLanguageCode,
                targetLanguageCode,
                ex.Message
            );

            throw new TranslateTextException(ex);
        }
    }
}
