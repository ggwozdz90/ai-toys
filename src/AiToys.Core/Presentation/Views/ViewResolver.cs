using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Core.Presentation.Views;

internal sealed class ViewResolver : IViewResolver
{
    private readonly Dictionary<string, (Type ViewType, Type ViewModelType)> routeMappings = [];

    public void RegisterView<TView, TViewModel>()
        where TView : IView<TViewModel>
        where TViewModel : IViewModel
    {
        var viewModelType = typeof(TViewModel);
        var viewType = typeof(TView);

        var routeName = viewType.Name;

        routeMappings[routeName] = (viewType, viewModelType);
    }

    public Type ResolveRouteView(string route)
    {
        ArgumentException.ThrowIfNullOrEmpty(route);

        if (!routeMappings.TryGetValue(route, out var typeMapping))
        {
            throw new ArgumentException($"Route '{route}' is not registered.", nameof(route));
        }

        return typeMapping.ViewType;
    }
}
