using System.IO.Abstractions;
using AiToys.SpeechToText.Domain.Enums;

namespace AiToys.SpeechToText.Domain.Models;

internal sealed class FileItemModel(string filePath, IFileSystem fileSystem)
{
    public string FilePath { get; } = filePath;
    public string FileName { get; } = fileSystem.Path.GetFileName(filePath);
    public FileItemStatus Status { get; private set; } = FileItemStatus.Added;
    public string Transcription { get; private set; } = string.Empty;

    public void SetStatus(FileItemStatus status)
    {
        Status = status;
    }

    public void SetTranscription(string transcription)
    {
        Transcription = transcription;
    }
}
