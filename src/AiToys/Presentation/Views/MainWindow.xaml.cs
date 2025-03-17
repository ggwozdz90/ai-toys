using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.Views;
using AiToys.Presentation.ViewModels;

namespace AiToys.Presentation.Views;

internal sealed partial class MainWindow : IView<MainViewModel>
{
    public MainWindow(MainViewModel viewModel, INavigationFrameProvider navigationFrameProvider)
    {
        InitializeComponent();

        AppTitleBar.ViewModel = viewModel.AppTitleBarViewModel;
        SetTitleBar(AppTitleBar);

        navigationFrameProvider.SetNavigationFrame(NavigationFrame);

        ViewModel = viewModel;
        ViewModel.NavigateToHome();
    }

    public MainViewModel ViewModel { get; set; }
}
