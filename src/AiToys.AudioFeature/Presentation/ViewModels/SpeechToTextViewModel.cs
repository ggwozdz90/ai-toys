using AiToys.AudioFeature.Constants;
using AiToys.Core.Presentation.ViewModels;

namespace AiToys.AudioFeature.Presentation.ViewModels;

internal sealed partial class SpeechToTextViewModel : ViewModelBase, IRouteAwareViewModel
{
    public string? ButtonText { get; set; } = "SpeechToTextViewModel";

    public string Route => RouteNames.SpeechToTextPage;
}
