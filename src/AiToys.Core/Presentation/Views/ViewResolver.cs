using AiToys.Core.Presentation.Services;

namespace AiToys.Core.Presentation.Views;

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
