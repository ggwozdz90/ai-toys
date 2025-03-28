using AiToys.Core.Presentation.Events;
using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Core.Presentation.Services;

/// <summary>
/// Contract for the navigation service.
/// This service is responsible for switching content in the main frame.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Occurs after navigation to a view has completed.
    /// </summary>
    event EventHandler<NavigatedEventArgs> Navigated;

    /// <summary>
    /// Gets a value indicating whether the navigation service can navigate back.
    /// </summary>
    bool CanNavigateBack { get; }

    /// <summary>
    /// Navigates back to the previous view.
    /// </summary>
    void NavigateBack();

    /// <summary>
    /// Navigates to the view associated with the specified route.
    /// </summary>
    /// <param name="route">The route name (view name) to navigate to.</param>
    void NavigateTo(string route);

    /// <summary>
    /// Gets the navigation items.
    /// </summary>
    /// <returns>The navigation items.</returns>
    IReadOnlyList<INavigationItemViewModel> GetNavigationItems();
}
