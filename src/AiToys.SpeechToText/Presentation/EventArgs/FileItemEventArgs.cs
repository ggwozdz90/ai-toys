using AiToys.SpeechToText.Presentation.ViewModels;

namespace AiToys.SpeechToText.Presentation.EventArgs;

internal sealed class FileItemEventArgs(FileItemViewModel fileItem) : System.EventArgs
{
    public FileItemViewModel FileItem { get; } = fileItem;
}
