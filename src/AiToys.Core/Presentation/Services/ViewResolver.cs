using AiToys.Core.Presentation.Contracts;

namespace AiToys.Core.Presentation.Services;

internal sealed class ViewResolver : IViewResolver
{
    private readonly Dictionary<Type, Type> viewModelToViewMappings = [];
    private readonly Dictionary<string, (Type ViewType, Type ViewModelType)> routeMappings = [];

    public void RegisterView<TView, TViewModel>()
        where TView : IView<TViewModel>
        where TViewModel : IViewModel
    {
        var viewModelType = typeof(TViewModel);
        var viewType = typeof(TView);

        var routeName = viewType.Name;

        viewModelToViewMappings[viewModelType] = viewType;
        routeMappings[routeName] = (viewType, viewModelType);
    }

    public Type ResolveViewType<TViewModel>()
        where TViewModel : IViewModel
    {
        var viewModelType = typeof(TViewModel);

        if (!viewModelToViewMappings.TryGetValue(viewModelType, out var viewType))
        {
            throw new InvalidOperationException($"No view registered for view model type {viewModelType.FullName}");
        }

        return viewType;
    }

    public (Type ViewType, Type ViewModelType) ResolveRoute(string route)
    {
        ArgumentException.ThrowIfNullOrEmpty(route);

        if (!routeMappings.TryGetValue(route, out var typeMapping))
        {
            throw new ArgumentException($"Route '{route}' is not registered.", nameof(route));
        }

        return typeMapping;
    }
}
