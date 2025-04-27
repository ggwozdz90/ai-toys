using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace Extensions.Hosting.WinUi;

internal sealed class WindowProvider(WinUiContext winUiContext) : IWindowProvider
{
    public Window GetMainWindow()
    {
        if (winUiContext.MainWindow is null)
        {
            throw new InvalidOperationException(
                "Main window is not available. Ensure this is called after the window has been initialized."
            );
        }

        return winUiContext.MainWindow;
    }

    public IntPtr GetMainWindowHandle()
    {
        if (winUiContext.MainWindow is null)
        {
            throw new InvalidOperationException(
                "Main window is not available. Ensure this is called after the window has been initialized."
            );
        }

        return WindowNative.GetWindowHandle(winUiContext.MainWindow);
    }
}
