using AiToys.Core.Presentation.Contracts;
using AiToys.HomeFeature.Constants;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml.Media;

namespace AiToys.Presentation.ViewModels;

internal sealed class MainViewModel(INavigationService navigationService) : IViewModel
{
    public bool ExtendsContentIntoTitleBar { get; set; } = true;

    public SystemBackdrop SystemBackdrop { get; set; } = new MicaBackdrop() { Kind = MicaKind.Base };

    public void NavigateToHome()
    {
        navigationService.NavigateToRoute(HomeRouteNames.HomePage);
    }
}
