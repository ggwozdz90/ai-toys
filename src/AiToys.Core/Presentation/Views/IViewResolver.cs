namespace AiToys.Core.Presentation.Views;

/// <summary>
/// Resolves views based on routes.
/// </summary>
public interface IViewResolver
{
    /// <summary>
    /// Resolves a route to a view type.
    /// </summary>
    /// <param name="route">The route to resolve.</param>
    /// <returns>The view type associated with the route.</returns>
    Type ResolveRouteView(string route);

    /// <summary>
    /// Registers a route-to-view mapping.
    /// </summary>
    /// <param name="route">The route to register.</param>
    /// <param name="viewType">The view type to associate with the route.</param>
    void RegisterRoute(string route, Type viewType);
}
