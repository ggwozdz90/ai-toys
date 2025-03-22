using AiToys.Presentation.ViewModels;

namespace AiToys.Presentation.Components.Organisms;

internal sealed partial class AppTitleBar
{
    public AppTitleBar() => InitializeComponent();

    public AppTitleBarViewModel ViewModel { get; set; } = null!;
}
