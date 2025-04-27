using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Application.UseCases;

internal interface IGetSupportedLanguagesUseCase
{
    Task<IReadOnlyList<LanguageModel>> ExecuteAsync(CancellationToken cancellationToken = default);
}

internal sealed class GetSupportedLanguagesUseCase(
    ISpeechToTextRepository speechToTextRepository,
    ILogger<GetSupportedLanguagesUseCase> logger
) : IGetSupportedLanguagesUseCase
{
    public async Task<IReadOnlyList<LanguageModel>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving supported languages");

        try
        {
            var languages = await speechToTextRepository
                .GetSupportedLanguagesAsync(cancellationToken)
                .ConfigureAwait(false);

            logger.LogInformation("Retrieved {Count} supported languages", languages.Count);

            return languages;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving supported languages: {ErrorMessage}", ex.Message);

            throw new GetSupportedLanguagesException(ex);
        }
    }
}
