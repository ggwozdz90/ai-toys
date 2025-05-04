using AiToys.SpeechToText.Domain.Enums;
using AiToys.SpeechToText.Domain.Events;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Application.Services;

internal interface IFileStatusNotifierService
{
    event EventHandler<FileStatusChangedEventArgs>? FileStatusChanged;

    void NotifyStatusChanged(string filePath, FileItemStatus status);
}

internal sealed class FileStatusNotifierService(ILogger<FileStatusNotifierService> logger) : IFileStatusNotifierService
{
    public event EventHandler<FileStatusChangedEventArgs>? FileStatusChanged;

    public void NotifyStatusChanged(string filePath, FileItemStatus status)
    {
        logger.LogDebug("File status changed: {FilePath}, {Status}", filePath, status);

        FileStatusChanged?.Invoke(this, new FileStatusChangedEventArgs(filePath, status));
    }
}
