using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Core.Presentation.Services;

/// <summary>
/// Provides a centralized service for managing and retrieving navigation items from multiple providers.
/// This service aggregates navigation items, sorts them by their defined order, and makes them available
/// for consumption by navigation UI components.
/// </summary>
public interface INavigationItemsService
{
    /// <summary>
    /// Retrieves all navigation items from the registered providers in their specified order.
    /// </summary>
    /// <returns>An immutable list of navigation items sorted by their order property.</returns>
    IReadOnlyList<INavigationItemViewModel> GetNavigationItems();
}
