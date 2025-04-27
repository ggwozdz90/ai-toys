using Microsoft.UI.Xaml;

namespace Extensions.Hosting.WinUi;

/// <summary>
/// Provides access to the main application window.
/// </summary>
public interface IWindowProvider
{
    /// <summary>
    /// Gets the main application window.
    /// </summary>
    /// <returns>The main application window.</returns>
    Window GetMainWindow();

    /// <summary>
    /// Gets the handle to the main application window.
    /// </summary>
    /// <returns>The handle to the main application window.</returns>
    IntPtr GetMainWindowHandle();
}
