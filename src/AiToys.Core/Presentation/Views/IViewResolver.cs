using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Core.Presentation.Views;

/// <summary>
/// Resolves the view type for a given view model.
/// Used by navigation service to resolve the view type for a given view model when navigating.
/// </summary>
public interface IViewResolver
{
    /// <summary>
    /// Registers the view type for a given view model in the view resolver.
    /// Registration is not affecting the IoC container and is used only for view resolution.
    /// </summary>
    /// <typeparam name="TView">The view type.</typeparam>
    /// <typeparam name="TViewModel">The view model type.</typeparam>
    void RegisterView<TView, TViewModel>()
        where TView : IView<TViewModel>
        where TViewModel : IViewModel;

    /// <summary>
    /// Resolves the view type for a given view model.
    /// </summary>
    /// <typeparam name="TViewModel">The view model type.</typeparam>
    /// <returns>The view type.</returns>
    Type ResolveViewType<TViewModel>()
        where TViewModel : IViewModel;

    /// <summary>
    /// Resolves view and view model types for a specified route.
    /// </summary>
    /// <param name="route">The route name (view name).</param>
    /// <returns>A tuple containing the view type and view model type.</returns>
    (Type ViewType, Type ViewModelType) ResolveRoute(string route);
}
