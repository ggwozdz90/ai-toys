using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Application.UseCases;

internal interface IGetDefaultFileExtensionsUseCase
{
    Task<string> ExecuteAsync(CancellationToken cancellationToken = default);
}

internal sealed class GetDefaultFileExtensionsUseCase(
    IFileExtensionsRepository fileExtensionsRepository,
    ILogger<GetDefaultFileExtensionsUseCase> logger
) : IGetDefaultFileExtensionsUseCase
{
    public async Task<string> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving default file extensions");

        try
        {
            var fileExtensions = await fileExtensionsRepository
                .GetDefaultFileExtensionsAsync(cancellationToken)
                .ConfigureAwait(false);

            logger.LogInformation("Retrieved default file extensions: {Extensions}", fileExtensions);

            return fileExtensions;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving default file extensions: {ErrorMessage}", ex.Message);
            throw new GetDefaultFileExtensionsException("Failed to retrieve default file extensions", ex);
        }
    }
}
