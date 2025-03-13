using AiToys.Core.Presentation.Contracts;

namespace AiToys.Core.Presentation.Services;

internal sealed class ViewResolver : IViewResolver
{
    private readonly Dictionary<Type, Type> viewModelToViewMappings = [];

    public void RegisterView<TView, TViewModel>()
        where TView : IView<TViewModel>
        where TViewModel : IViewModel
    {
        viewModelToViewMappings[typeof(TViewModel)] = typeof(TView);
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
}
