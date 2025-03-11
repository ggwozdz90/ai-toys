using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

namespace Extensions.Hosting.WinUi;

internal sealed class WinUIContext
{
    public Application? Application { get; set; }

    public Window? MainWindow { get; set; }

    public required Type MainWindowType { get; set; }

    public DispatcherQueue? DispatcherQueue { get; set; }

    public bool IsLifetimeLinked { get; set; }

    public bool IsRunning { get; set; }
}
