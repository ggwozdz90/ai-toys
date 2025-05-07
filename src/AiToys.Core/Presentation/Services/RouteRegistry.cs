using AiToys.Core.Presentation.Options;
using AiToys.Core.Presentation.Views;
using Microsoft.Extensions.Options;

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

internal sealed class RouteRegistry : IRouteRegistry
{
    private readonly Dictionary<string, Type> routes = new(StringComparer.OrdinalIgnoreCase);

    public RouteRegistry(IOptions<RouteRegistryOptions> options)
    {
        foreach (var mapping in options.Value.RouteMappings)
        {
            RegisterRoute(mapping.Key, mapping.Value);
        }
    }

    public void RegisterRoute(string route, Type viewType)
    {
        if (!typeof(IView).IsAssignableFrom(viewType))
        {
            throw new ArgumentException($"Type '{viewType.Name}' does not implement IView.", nameof(viewType));
        }

        routes[route] = viewType;
    }

    public IReadOnlyDictionary<string, Type> GetRoutes() => routes;
}
