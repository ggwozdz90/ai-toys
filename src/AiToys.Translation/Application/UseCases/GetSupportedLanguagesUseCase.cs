using AiToys.Translation.Domain.Exceptions;
using AiToys.Translation.Domain.Models;
using AiToys.Translation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiToys.Translation.Application.UseCases;

internal interface IGetSupportedLanguagesUseCase
{
    Task<IReadOnlyList<LanguageModel>> ExecuteAsync(CancellationToken cancellationToken = default);
}

internal sealed class GetSupportedLanguagesUseCase(
    ITranslationRepository translationRepository,
    ILogger<GetSupportedLanguagesUseCase> logger
) : IGetSupportedLanguagesUseCase
{
    public async Task<IReadOnlyList<LanguageModel>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving supported languages");

        try
        {
            var languages = await translationRepository
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
