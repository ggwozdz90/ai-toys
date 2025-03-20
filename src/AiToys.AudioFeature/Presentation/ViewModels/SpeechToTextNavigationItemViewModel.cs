using AiToys.AudioFeature.Constants;
using AiToys.Core.Presentation.ViewModels;

namespace AiToys.AudioFeature.Presentation.ViewModels;

internal class SpeechToTextNavigationItemViewModel : INavigationItemViewModel
{
    public string Label => AudioPageNames.SpeechToTextPage;
    public string Route => AudioRouteNames.SpeechToTextPage;
    public int Order => 200;
    public string IconKey => AudioIconKeys.SpeechToText;
}
