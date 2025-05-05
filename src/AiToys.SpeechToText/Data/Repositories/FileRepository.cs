using System.IO.Abstractions;
using System.Text;
using AiToys.SpeechToText.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Data.Repositories;

internal sealed class FileRepository(IFileSystem fileSystem, ILogger<FileRepository> logger) : IFileRepository
{
    public async Task SaveFileAsync(
        string filePath,
        string transcription,
        string language,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation("Saving transcription file to: {FilePath} in language: {Language}", filePath, language);

        try
        {
            var directory = fileSystem.Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directory) && !fileSystem.Directory.Exists(directory))
            {
                fileSystem.Directory.CreateDirectory(directory);
            }

            await fileSystem
                .File.WriteAllTextAsync(filePath, transcription, Encoding.UTF8, cancellationToken)
                .ConfigureAwait(false);

            logger.LogInformation(
                "Successfully saved transcription file to: {FilePath} in language: {Language}",
                filePath,
                language
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error saving transcription file to: {FilePath} in language: {Language}: {ErrorMessage}",
                filePath,
                language,
                ex.Message
            );

            throw;
        }
    }
}
