namespace AiToys.Core.Presentation.Services;

/// <summary>
/// Provides data for the navigated events.
/// </summary>
public class NavigatedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NavigatedEventArgs"/> class.
    /// </summary>
    /// <param name="route">The navigated route.</param>
    public NavigatedEventArgs(string route)
    {
        Route = route ?? throw new ArgumentNullException(nameof(route));
    }

    /// <summary>
    /// Gets the route that was navigated to.
    /// </summary>
    public string Route { get; }
}
