using AiToys.Core.Presentation.Contracts;
using AiToys.Presentation.ViewModels;

namespace AiToys.Presentation.Views;

internal sealed partial class MainWindow : IView<MainViewModel>
{
    public MainWindow(MainViewModel viewModel, INavigationFrameProvider navigationFrameProvider)
    {
        InitializeComponent();
        navigationFrameProvider.SetNavigationFrame(NavigationFrame);

        ViewModel = viewModel;
        ViewModel.NavigateToHome();
    }

    public MainViewModel ViewModel { get; set; }
}
