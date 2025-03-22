using AiToys.Core.Presentation.Extensions;
using AiToys.Core.Presentation.Views;
using Microsoft.Extensions.Options;

namespace AiToys.Core.Presentation.Services;

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
