using AiToys.Presentation.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace AiToys.Presentation.Components.Organisms;

internal sealed partial class AppTitleBar : UserControl
{
    public AppTitleBar() => InitializeComponent();

    public AppTitleBarViewModel ViewModel { get; set; } = null!;
}
