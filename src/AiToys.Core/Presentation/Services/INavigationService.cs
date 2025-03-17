using AiToys.Core.Presentation.ViewModels;
using AiToys.Core.Presentation.Views;

namespace AiToys.Core.Presentation.Services;

/// <summary>
/// Contract for the navigation service.
/// This service is responsible for switching content in the main frame.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Gets a value indicating whether the navigation service can navigate back.
    /// </summary>
    bool CanGoBack { get; }

    /// <summary>
    /// Navigates back to the previous view.
    /// </summary>
    void GoBack();

    /// <summary>
    /// Navigates to the view represented by the specified view model.
    /// View and view model must be registered in the dependency injection container and must be configured in the <see cref="IViewResolver"/>.
    /// </summary>
    /// <typeparam name="TViewModel">The view model to navigate to.</typeparam>
    void NavigateTo<TViewModel>()
        where TViewModel : IViewModel;

    /// <summary>
    /// Navigates to the view associated with the specified route.
    /// </summary>
    /// <param name="route">The route name (view name) to navigate to.</param>
    void NavigateToRoute(string route);
}
