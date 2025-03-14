using Microsoft.UI.Xaml.Controls;

namespace AiToys.Core.Presentation.Contracts;

/// <summary>
/// Contract for providing and managing the navigation frame.
/// </summary>
public interface INavigationFrameProvider
{
    /// <summary>
    /// Sets the main frame that will be used for navigation.
    /// </summary>
    /// <param name="frame">The main frame.</param>
    void SetNavigationFrame(Frame frame);
}
