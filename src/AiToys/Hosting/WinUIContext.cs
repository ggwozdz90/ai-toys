using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

namespace AiToys.Hosting;

internal sealed class WinUIContext
{
    public Window? MainWindow { get; set; }

    public Application? Application { get; set; }

    public required Type MainWindowType { get; set; }

    public DispatcherQueue? DispatcherQueue { get; set; }

    public bool IsLifetimeLinked { get; set; }

    public bool IsRunning { get; set; }
}
