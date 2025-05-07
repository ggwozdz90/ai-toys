using System.IO.Abstractions;
using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Application.UseCases;

internal interface ISaveTranscriptionUseCase
{
    Task ExecuteAsync(
        string filePath,
        string transcription,
        string language,
        CancellationToken cancellationToken = default
    );
}

internal sealed class SaveTranscriptionUseCase(
    IFileRepository fileRepository,
    IFileSystem fileSystem,
    ILogger<SaveTranscriptionUseCase> logger
) : ISaveTranscriptionUseCase
{
    public async Task ExecuteAsync(
        string filePath,
        string transcription,
        string language,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
        }

        if (string.IsNullOrWhiteSpace(transcription))
        {
            throw new ArgumentException("Transcription cannot be null or empty", nameof(transcription));
        }

        if (string.IsNullOrWhiteSpace(language))
        {
            throw new ArgumentException("Language cannot be null or empty", nameof(language));
        }

        var srtFilePath = GenerateSrtFilePath(filePath, language);

        logger.LogInformation(
            "Saving transcription to SRT file: {SrtFilePath} in language: {Language}",
            srtFilePath,
            language
        );

        try
        {
            await fileRepository
                .SaveFileAsync(srtFilePath, transcription, language, cancellationToken)
                .ConfigureAwait(false);

            logger.LogInformation(
                "Successfully saved transcription to SRT file: {SrtFilePath} in language: {Language}",
                srtFilePath,
                language
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error saving transcription to SRT file: {SrtFilePath} in language: {Language}: {ErrorMessage}",
                srtFilePath,
                language,
                ex.Message
            );

            throw new SaveTranscriptionException("Failed to save transcription to SRT file", ex);
        }
    }

    private string GenerateSrtFilePath(string originalFilePath, string targetLanguage)
    {
        var directory = fileSystem.Path.GetDirectoryName(originalFilePath) ?? string.Empty;
        var filenameWithoutExtension = fileSystem.Path.GetFileNameWithoutExtension(originalFilePath);

        var srtFileName = string.IsNullOrWhiteSpace(targetLanguage)
            ? $"{filenameWithoutExtension}.srt"
            : $"{filenameWithoutExtension}.{targetLanguage}.srt";

        return fileSystem.Path.Combine(directory, srtFileName);
    }
}
