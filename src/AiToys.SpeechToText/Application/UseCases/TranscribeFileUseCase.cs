using System.IO.Abstractions;
using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Application.UseCases;

internal interface ITranscribeFileUseCase
{
    Task<string> ExecuteAsync(
        string filePath,
        string sourceLanguage,
        string? targetLanguage = null,
        CancellationToken cancellationToken = default
    );
}

internal sealed class TranscribeFileUseCase(
    ISpeechToTextRepository speechToTextRepository,
    IFileSystem fileSystem,
    ILogger<TranscribeFileUseCase> logger
) : ITranscribeFileUseCase
{
    public async Task<string> ExecuteAsync(
        string filePath,
        string sourceLanguage,
        string? targetLanguage = null,
        CancellationToken cancellationToken = default
    )
    {
        ValidateInputParameters(filePath, sourceLanguage, targetLanguage);

        var shouldTranslate = ShouldTranslate(sourceLanguage, targetLanguage);

        logger.LogInformation(
            "Starting processing for file: {FilePath} from {SourceLanguage}{TargetLanguage}",
            filePath,
            sourceLanguage,
            shouldTranslate ? $" to {targetLanguage}" : string.Empty
        );

        try
        {
            var result = await ProcessFileAsync(
                    filePath,
                    sourceLanguage,
                    targetLanguage,
                    shouldTranslate,
                    cancellationToken
                )
                .ConfigureAwait(false);

            logger.LogInformation("Successfully completed processing file: {FilePath}", filePath);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error during processing file: {FilePath} from {SourceLanguage}{TargetLanguage}: {ErrorMessage}",
                filePath,
                sourceLanguage,
                shouldTranslate ? $" to {targetLanguage}" : string.Empty,
                ex.Message
            );
            throw new TranscribeFileException(ex);
        }
    }

    private static bool ShouldTranslate(string sourceLanguage, string? targetLanguage)
    {
        return !string.IsNullOrWhiteSpace(targetLanguage)
            && !string.Equals(sourceLanguage, targetLanguage, StringComparison.Ordinal);
    }

    private void ValidateInputParameters(string filePath, string sourceLanguage, string? targetLanguage)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
        }

        if (!fileSystem.File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified file does not exist", filePath);
        }

        if (string.IsNullOrWhiteSpace(sourceLanguage))
        {
            throw new ArgumentException("Source language code cannot be null or empty", nameof(sourceLanguage));
        }

        if (targetLanguage != null && string.IsNullOrWhiteSpace(targetLanguage))
        {
            throw new ArgumentException("Target language code cannot be null or empty", nameof(targetLanguage));
        }
    }

    private Task<string> ProcessFileAsync(
        string filePath,
        string sourceLanguage,
        string? targetLanguage,
        bool shouldTranslate,
        CancellationToken cancellationToken
    )
    {
        if (shouldTranslate && targetLanguage != null)
        {
            return speechToTextRepository.TranscribeAndTranslateFileAsync(
                filePath,
                sourceLanguage,
                targetLanguage,
                cancellationToken
            );
        }

        return speechToTextRepository.TranscribeFileAsync(filePath, sourceLanguage, cancellationToken);
    }
}
