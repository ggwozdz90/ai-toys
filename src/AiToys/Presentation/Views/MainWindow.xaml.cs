using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.Views;
using AiToys.Presentation.ViewModels;

namespace AiToys.Presentation.Views;

internal sealed partial class MainWindow : IView<MainViewModel>
{
    public MainWindow(MainViewModel viewModel, INavigationFrameProvider navigationFrameProvider)
    {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        AppTitleBar.ViewModel = viewModel.AppTitleBarViewModel;
        SetTitleBar(AppTitleBar);

        navigationFrameProvider.SetNavigationFrame(NavigationFrame);

        ViewModel = viewModel;

        var firstRoute = ViewModel.NavigationItems.OrderBy(item => item.Order).FirstOrDefault()?.Route;
        ViewModel.Navigate(firstRoute);
    }

    public MainViewModel ViewModel { get; set; }
}
