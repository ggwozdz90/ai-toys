using AiToys.Core.Presentation.ViewModels;
using AiToys.HomeFeature.Constants;

namespace AiToys.HomeFeature.Presentation.ViewModels;

internal sealed class HomeNavigationItemViewModel : INavigationItemViewModel
{
    public string Label => PageNames.HomePage;
    public string Route => HomeRouteNames.HomePage;
    public int Order => 100;
    public string IconKey => IconKeys.Home;
}
