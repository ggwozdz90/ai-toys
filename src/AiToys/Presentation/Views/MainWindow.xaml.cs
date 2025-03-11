using AiToys.Presentation.Contracts;
using AiToys.Presentation.ViewModels;

namespace AiToys.Presentation.Views;

internal sealed partial class MainWindow : IView<MainViewModel>
{
    public MainWindow(MainViewModel viewModel, INavigationService navigationService)
    {
        InitializeComponent();
        navigationService.SetNavigationFrame(NavigationFrame);

        ViewModel = viewModel;
        ViewModel.NavigateTo<HomeViewModel>();
    }

    public MainViewModel ViewModel { get; set; }
}
