using AiToys.Core.Presentation.Views;
using AiToys.HomeFeature.Presentation.ViewModels;

namespace AiToys.HomeFeature.Presentation.Views;

internal sealed partial class HomePage : IView<HomeViewModel>
{
    public HomePage() => InitializeComponent();

    public HomeViewModel ViewModel { get; set; } = null!;
}
