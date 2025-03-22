namespace AiToys.Core.Presentation.Services;

/// <summary>
/// Provides data for the navigated events.
/// </summary>
public class NavigatedEventArgs(string route) : EventArgs
{
    /// <summary>
    /// Gets the route that was navigated to.
    /// </summary>
    public string Route { get; } = route ?? throw new ArgumentNullException(nameof(route));
}
