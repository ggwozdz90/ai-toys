using AiToys.SpeechToText.Domain.Enums;

namespace AiToys.SpeechToText.Domain.Models;

internal sealed class FileItemModel(string filePath)
{
    public string FilePath { get; } = filePath;
    public string FileName { get; } = Path.GetFileName(filePath);
    public FileItemStatus Status { get; private set; } = FileItemStatus.Pending;

    public void SetStatus(FileItemStatus status)
    {
        Status = status;
    }
}
