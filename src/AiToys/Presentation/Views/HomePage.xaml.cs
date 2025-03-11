using AiToys.Presentation.Contracts;
using AiToys.Presentation.ViewModels;

namespace AiToys.Presentation.Views;

internal sealed partial class HomePage : IView<HomeViewModel>
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }

    public HomeViewModel ViewModel { get; set; }
}
