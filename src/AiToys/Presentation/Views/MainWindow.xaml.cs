using Extensions.Hosting.WinUi;

namespace AiToys.Presentation.Views;

internal sealed partial class MainWindow : IWinUiWindow
{
    public MainWindow(IMainViewModel viewModel)
    {
        ViewModel = viewModel;
        ExtendsContentIntoTitleBar = true;
        InitializeComponent();
    }

    public IMainViewModel ViewModel { get; set; }
}
