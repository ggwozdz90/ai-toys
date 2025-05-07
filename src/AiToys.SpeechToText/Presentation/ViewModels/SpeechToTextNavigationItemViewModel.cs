using AiToys.Core.Presentation.ViewModels;
using AiToys.SpeechToText.Constants;

namespace AiToys.SpeechToText.Presentation.ViewModels;

internal sealed class SpeechToTextNavigationItemViewModel : INavigationItemViewModel
{
    public string Label => PageLabels.SpeechToTextPage;
    public string Route => RouteNames.SpeechToTextPage;
    public int Order => 200;
    public string IconKey => IconKeys.SpeechToText;
    public string Description => PageDescriptions.SpeechToTextPage;
}
