using AiToys.Core.Presentation.ViewModels;
using AiToys.HomeFeature.Constants;

namespace AiToys.HomeFeature.Presentation.ViewModels;

internal class HomeNavigationItemViewModel : INavigationItemViewModel
{
    public string Label => HomePageNames.HomePage;
    public string Route => HomeRouteNames.HomePage;
    public int Order => 100;
}
