using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.Views;
using AiToys.Presentation.ViewModels;

namespace AiToys.Presentation.Views;

internal sealed partial class MainWindow : IView<MainViewModel>
{
    public MainWindow(MainViewModel viewModel, INavigationFrameProvider navigationFrameProvider)
    {
        ViewModel = viewModel;
        InitializeComponent();
        InitializeAppTitleBar();
        InitializeNavigationView(navigationFrameProvider);
    }

    public MainViewModel ViewModel { get; set; }

    private void InitializeAppTitleBar()
    {
        ExtendsContentIntoTitleBar = true;
        AppTitleBar.ViewModel = ViewModel.AppTitleBarViewModel;
        SetTitleBar(AppTitleBar);
    }

    private void InitializeNavigationView(INavigationFrameProvider navigationFrameProvider)
    {
        navigationFrameProvider.SetNavigationFrame(NavigationFrame);

        var firstRoute = ViewModel.NavigationItems.Any() ? ViewModel.NavigationItems[0].Route : null;
        ViewModel.NavigateTo(firstRoute);
    }
}
