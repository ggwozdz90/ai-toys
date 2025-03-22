namespace AiToys.Core.Presentation.Services;

/// <summary>
/// Service that manages route to view type mappings.
/// </summary>
internal interface IRouteRegistry
{
    /// <summary>
    /// Registers a route to view type mapping.
    /// </summary>
    /// <param name="route">The route to register.</param>
    /// <param name="viewType">The view type associated with the route.</param>
    void RegisterRoute(string route, Type viewType);

    /// <summary>
    /// Gets all registered routes.
    /// </summary>
    /// <returns>A collection of route to view type mappings.</returns>
    IReadOnlyDictionary<string, Type> GetRoutes();
}
