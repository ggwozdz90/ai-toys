using AiToys.Core.Presentation.Views;
using AiToys.Home.Presentation.ViewModels;

namespace AiToys.Home.Presentation.Views;

internal sealed partial class HomePage : IView<HomeViewModel>
{
    public HomePage() => InitializeComponent();

    public HomeViewModel ViewModel { get; set; } = null!;
}
