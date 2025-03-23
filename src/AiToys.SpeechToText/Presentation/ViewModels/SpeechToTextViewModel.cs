using AiToys.Core.Presentation.ViewModels;
using AiToys.SpeechToText.Constants;

namespace AiToys.SpeechToText.Presentation.ViewModels;

internal sealed partial class SpeechToTextViewModel : ViewModelBase, IRouteAwareViewModel
{
    public string Route => RouteNames.SpeechToTextPage;

    public string? ButtonText { get; set; } = "SpeechToTextViewModel";
}
