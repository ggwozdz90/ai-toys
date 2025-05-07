using AiToys.Core.Presentation.Views;
using AiToys.SpeechToText.Presentation.ViewModels;

namespace AiToys.SpeechToText.Presentation.Views;

internal sealed partial class SpeechToTextPage : IView<SpeechToTextViewModel>
{
    public SpeechToTextPage() => InitializeComponent();

    public SpeechToTextViewModel ViewModel { get; set; } = null!;
}
