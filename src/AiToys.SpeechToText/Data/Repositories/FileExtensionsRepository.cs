using AiToys.SpeechToText.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Data.Repositories;

internal sealed class FileExtensionsRepository(ILogger<FileExtensionsRepository> logger) : IFileExtensionsRepository
{
    public Task<string> GetDefaultFileExtensionsAsync(CancellationToken cancellationToken = default)
    {
        // Hardcoded value for now, in the future will be loaded from settings file
        const string defaultExtensions = "mp4, avi, mov, wmv";

        logger.LogInformation("Retrieved default file extensions: {Extensions}", defaultExtensions);

        return Task.FromResult(defaultExtensions);
    }
}
