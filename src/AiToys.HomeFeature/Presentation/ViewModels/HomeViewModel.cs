using AiToys.Core.Presentation.ViewModels;
using AiToys.HomeFeature.Constants;

namespace AiToys.HomeFeature.Presentation.ViewModels;

internal sealed partial class HomeViewModel : ViewModelBase, IRouteAwareViewModel
{
    public string? ButtonText { get; set; } = "HomeViewModel";

    public string Route => HomeRouteNames.HomePage;
}
