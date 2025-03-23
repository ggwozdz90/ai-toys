using AiToys.Core.Presentation.ViewModels;
using AiToys.Home.Constants;

namespace AiToys.Home.Presentation.ViewModels;

internal sealed partial class HomeViewModel : ViewModelBase, IRouteAwareViewModel
{
    public string Route => RouteNames.HomePage;
    public string? ButtonText { get; set; } = "HomeViewModel";
}
