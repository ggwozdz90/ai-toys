using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Core.Presentation.Services;

/// <summary>
/// Defines an interface for components that provide navigation items for the application.
/// Each feature module can implement this interface to contribute its navigation items
/// to the main navigation structure without direct dependencies between features.
/// </summary>
public interface INavigationItemsProvider
{
    /// <summary>
    /// Provides a collection of navigation items for a specific feature or module.
    /// </summary>
    /// <returns>A collection of navigation item view models representing navigable destinations.</returns>
    IEnumerable<INavigationItemViewModel> GetNavigationItems();
}
