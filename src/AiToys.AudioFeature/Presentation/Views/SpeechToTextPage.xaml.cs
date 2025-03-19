using AiToys.AudioFeature.Presentation.ViewModels;
using AiToys.Core.Presentation.Views;

namespace AiToys.AudioFeature.Presentation.Views;

internal sealed partial class SpeechToTextPage : IView<SpeechToTextViewModel>
{
    public SpeechToTextPage() => InitializeComponent();

    public SpeechToTextViewModel ViewModel { get; set; } = null!;
}
