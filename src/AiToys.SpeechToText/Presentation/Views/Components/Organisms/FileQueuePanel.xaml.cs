using AiToys.SpeechToText.Presentation.ViewModels;

namespace AiToys.SpeechToText.Presentation.Views.Components.Organisms;

internal sealed partial class FileQueuePanel
{
    public FileQueuePanel() => InitializeComponent();

    public FileQueueViewModel ViewModel => (FileQueueViewModel)DataContext;
}
