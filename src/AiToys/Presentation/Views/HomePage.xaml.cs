using AiToys.Core.Presentation.Contracts;
using AiToys.Presentation.ViewModels;

namespace AiToys.Presentation.Views;

internal sealed partial class HomePage : IView<HomeViewModel>
{
    public HomePage() => InitializeComponent();

    public HomeViewModel ViewModel { get; set; } = null!;
}
