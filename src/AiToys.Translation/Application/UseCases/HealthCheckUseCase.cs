using AiToys.Translation.Domain.Exceptions;
using AiToys.Translation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiToys.Translation.Application.UseCases;

internal interface IHealthCheckUseCase
{
    Task<bool> ExecuteAsync(CancellationToken cancellationToken = default);
}

internal sealed class HealthCheckUseCase(
    ITranslationRepository speechToTextRepository,
    ILogger<HealthCheckUseCase> logger
) : IHealthCheckUseCase
{
    public async Task<bool> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Executing health check use case");

            var result = await speechToTextRepository.HealthCheckAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Health check completed with result: {Result}", result);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during health check: {ErrorMessage}", ex.Message);
            throw new HealthCheckException(ex);
        }
    }
}
