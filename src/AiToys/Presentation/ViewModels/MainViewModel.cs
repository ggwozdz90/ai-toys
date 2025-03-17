using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;
using AiToys.HomeFeature.Constants;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml.Media;

namespace AiToys.Presentation.ViewModels;

internal sealed partial class MainViewModel(
    INavigationService navigationService,
    AppTitleBarViewModel appTitleBarViewModel
) : ViewModelBase
{
    public AppTitleBarViewModel AppTitleBarViewModel { get; } = appTitleBarViewModel;

    public bool ExtendsContentIntoTitleBar { get; set; } = true;

    public SystemBackdrop SystemBackdrop { get; set; } = new MicaBackdrop() { Kind = MicaKind.Base };

    public void NavigateToHome()
    {
        navigationService.NavigateToRoute(HomeRouteNames.HomePage);
    }
}
