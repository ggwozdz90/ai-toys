using AiToys.Core.Presentation.Services;

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
}

internal sealed class ViewResolver(IRouteRegistry routeRegistry) : IViewResolver
{
    public Type ResolveRouteView(string route)
    {
        ArgumentException.ThrowIfNullOrEmpty(route);

        var routes = routeRegistry.GetRoutes();

        if (!routes.TryGetValue(route, out var viewType))
        {
            throw new ArgumentException($"Route '{route}' is not registered.", nameof(route));
        }

        return viewType;
    }
}
