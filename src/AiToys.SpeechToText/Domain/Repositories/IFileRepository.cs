namespace AiToys.SpeechToText.Domain.Repositories;

internal interface IFileRepository
{
    Task SaveFileAsync(
        string filePath,
        string transcription,
        string language,
        CancellationToken cancellationToken = default
    );
}
