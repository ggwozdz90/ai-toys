namespace AiToys.SpeechToText.Domain.Repositories;

internal interface IFileExtensionsRepository
{
    Task<string> GetDefaultFileExtensionsAsync(CancellationToken cancellationToken = default);
}
