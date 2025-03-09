using Extensions.Hosting.WinUi;

namespace AiToys.Presentation.ViewModels;

internal sealed class MainViewModel(ShellViewModel shellViewModel) : IMainViewModel
{
    public object ShellViewModel { get; } = shellViewModel;
}
