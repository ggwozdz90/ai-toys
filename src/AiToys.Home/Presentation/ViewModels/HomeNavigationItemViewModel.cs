using AiToys.Core.Presentation.ViewModels;
using AiToys.Home.Constants;

namespace AiToys.Home.Presentation.ViewModels;

internal sealed class HomeNavigationItemViewModel : INavigationItemViewModel
{
    public string Label => PageLabels.HomePage;
    public string Route => RouteNames.HomePage;
    public int Order => 100;
    public string IconKey => IconKeys.Home;
}
