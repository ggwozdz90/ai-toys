using AiToys.AudioFeature.Constants;
using AiToys.Core.Presentation.ViewModels;

namespace AiToys.AudioFeature.Presentation.ViewModels;

internal class SpeechToTextNavigationItemViewModel : INavigationItemViewModel
{
    public string Label => PageNames.SpeechToTextPage;
    public string Route => RouteNames.SpeechToTextPage;
    public int Order => 200;
    public string IconKey => IconKeys.SpeechToText;
}
