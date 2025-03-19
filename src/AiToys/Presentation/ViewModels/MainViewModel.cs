using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;
using AiToys.HomeFeature.Constants;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml.Media;

namespace AiToys.Presentation.ViewModels;

internal sealed partial class MainViewModel(
    INavigationService navigationService,
    INavigationItemsService navigationItemsService,
    AppTitleBarViewModel appTitleBarViewModel
) : ViewModelBase
{
    private INavigationItemViewModel? selectedItem;

    public AppTitleBarViewModel AppTitleBarViewModel { get; } = appTitleBarViewModel;

    public bool ExtendsContentIntoTitleBar { get; set; } = true;

    public SystemBackdrop SystemBackdrop { get; set; } = new MicaBackdrop() { Kind = MicaKind.Base };

    public IReadOnlyList<INavigationItemViewModel> NavigationItems { get; } =
        navigationItemsService.GetNavigationItems();

    public INavigationItemViewModel? SelectedNavigationItem
    {
        get => selectedItem;
        set
        {
            if (SetProperty(ref selectedItem, value) && value != null)
            {
                navigationService.NavigateToRoute(value.Route);
            }
        }
    }

    public void NavigateToHome()
    {
        navigationService.NavigateToRoute(HomeRouteNames.HomePage);
    }
}
