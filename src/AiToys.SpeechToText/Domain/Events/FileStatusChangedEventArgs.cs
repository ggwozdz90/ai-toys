using AiToys.SpeechToText.Domain.Enums;

namespace AiToys.SpeechToText.Domain.Events;

internal sealed class FileStatusChangedEventArgs(string filePath, FileItemStatus status) : EventArgs
{
    public string FilePath { get; } = filePath;
    public FileItemStatus Status { get; } = status;
}
