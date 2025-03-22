using AiToys.Core.Presentation.ViewModels;
using AiToys.HomeFeature.Constants;

namespace AiToys.HomeFeature.Presentation.ViewModels;

internal sealed partial class HomeViewModel : ViewModelBase, IRouteAwareViewModel
{
    public string Route => RouteNames.HomePage;
    public string? ButtonText { get; set; } = "HomeViewModel";
}
