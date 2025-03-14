using AiToys.Core.Presentation.Contracts;
using AiToys.HomeFeature.Constants;
using AiToys.Presentation.ViewModels;

namespace AiToys.Presentation.Views;

internal sealed partial class MainWindow : IView<MainViewModel>
{
    public MainWindow(MainViewModel viewModel, INavigationService navigationService)
    {
        InitializeComponent();
        navigationService.SetNavigationFrame(NavigationFrame);

        ViewModel = viewModel;
        navigationService.NavigateToRoute(HomeRouteNames.HomePage);
    }

    public MainViewModel ViewModel { get; set; }
}
