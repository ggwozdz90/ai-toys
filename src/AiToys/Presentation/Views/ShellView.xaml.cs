using AiToys.Presentation.ViewModels;
using ReactiveUI;
using Splat;

namespace AiToys.Presentation.Views;

internal partial class ShellViewBase : ReactiveUserControl<ShellViewModel>;

internal sealed partial class ShellView
{
    public ShellView()
    {
        ViewModel = Locator.Current.GetService<ShellViewModel>();
        InitializeComponent();
    }
}
