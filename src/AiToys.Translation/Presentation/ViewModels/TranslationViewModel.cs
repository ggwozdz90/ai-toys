using AiToys.Core.Presentation.ViewModels;
using AiToys.Translation.Constants;

namespace AiToys.Translation.Presentation.ViewModels;

internal sealed partial class TranslationViewModel : ViewModelBase, IRouteAwareViewModel
{
    public string Route => RouteNames.TranslationPage;

    public string? ButtonText { get; set; } = "TranslationViewModel";
}
