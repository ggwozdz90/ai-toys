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

        NavigateCommand = new RelayCommand<string>(Navigate);
    }

    public AppTitleBarViewModel AppTitleBarViewModel { get; }

    public INavigationItemViewModel? SelectedNavigationItem
    {
        get => selectedItem;
        set => SetProperty(ref selectedItem, value);
    }

    public IReadOnlyList<INavigationItemViewModel> NavigationItems { get; }

    public ICommand NavigateCommand { get; }

    public void Navigate(string? route)
    {
        if (string.IsNullOrEmpty(route))
        {
            return;
        }

        navigationService.NavigateTo(route);
    }

    private void OnNavigated(object? sender, NavigatedEventArgs args)
    {
        if (string.IsNullOrEmpty(args.Route))
        {
            return;
        }

        var matchingItem = NavigationItems.FirstOrDefault(item => item.Route == args.Route);

        if (matchingItem != null && !Equals(SelectedNavigationItem, matchingItem))
        {
            SelectedNavigationItem = matchingItem;
        }
    }
}
