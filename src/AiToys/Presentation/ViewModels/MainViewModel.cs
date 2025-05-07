using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.Events;
using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;
using Extensions.Hosting.WinUi;

namespace AiToys.Presentation.ViewModels;

internal sealed partial class MainViewModel : ViewModelBase
{
    private readonly INavigationService navigationService;

    private INavigationItemViewModel? selectedItem;

    public MainViewModel(IDispatcherService dispatcherService, INavigationService navigationService)
        : base(dispatcherService)
    {
        this.navigationService = navigationService;
        AppTitleBarViewModel = new AppTitleBarViewModel(dispatcherService, navigationService);

        NavigationItems = navigationService.GetNavigationItems();
        navigationService.Navigated += OnNavigated;

        NavigateCommand = new RelayCommand<string>(NavigateTo);
    }

    public AppTitleBarViewModel AppTitleBarViewModel { get; }
    public IReadOnlyList<INavigationItemViewModel> NavigationItems { get; }
    public ICommandBase NavigateCommand { get; }

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

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            navigationService.Navigated -= OnNavigated;
            NavigateCommand.Dispose();
            AppTitleBarViewModel.Dispose();
        }

        base.Dispose(disposing);
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
