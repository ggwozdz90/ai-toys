using System.Windows.Input;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Presentation.ViewModels;

internal sealed partial class MainViewModel : ViewModelBase
{
    private readonly INavigationService navigationService;

    private INavigationItemViewModel? selectedItem;

    public MainViewModel(
        INavigationService navigationService,
        INavigationItemsService navigationItemsService,
        AppTitleBarViewModel appTitleBarViewModel
    )
    {
        this.navigationService = navigationService;
        AppTitleBarViewModel = appTitleBarViewModel;

        NavigationItems = navigationItemsService.GetNavigationItems();
        navigationService.Navigated += OnNavigated;

        NavigateCommand = new RelayCommand<string>(NavigateTo);
    }

    public AppTitleBarViewModel AppTitleBarViewModel { get; }
    public IReadOnlyList<INavigationItemViewModel> NavigationItems { get; }
    public ICommand NavigateCommand { get; }

    public INavigationItemViewModel? SelectedNavigationItem
    {
        get => selectedItem;
        set => SetProperty(ref selectedItem, value);
    }

    public void NavigateTo(string? route)
    {
        if (string.IsNullOrEmpty(route))
        {
            return;
        }

        navigationService.NavigateTo(route);
    }

    private void OnNavigated(object? sender, NavigatedEventArgs navigatedEventArgs)
    {
        if (string.IsNullOrEmpty(navigatedEventArgs.Route))
        {
            return;
        }

        var matchingItem = NavigationItems.FirstOrDefault(item =>
            string.Equals(item.Route, navigatedEventArgs.Route, StringComparison.Ordinal)
        );

        if (matchingItem != null && !Equals(SelectedNavigationItem, matchingItem))
        {
            SelectedNavigationItem = matchingItem;
        }
    }
}
