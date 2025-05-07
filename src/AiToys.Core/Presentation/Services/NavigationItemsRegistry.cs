using AiToys.Core.Presentation.Options;
using AiToys.Core.Presentation.ViewModels;
using Microsoft.Extensions.Options;

namespace AiToys.Core.Presentation.Services;

/// <summary>
/// Service that provides navigation items.
/// </summary>
public interface INavigationItemsRegistry
{
    /// <summary>
    /// Registers a navigation item type.
    /// </summary>
    /// <param name="navigationItemType">The navigation item type to register.</param>
    void RegisterNavigationItem(Type navigationItemType);

    /// <summary>
    /// Gets navigation items.
    /// </summary>
    /// <returns>A collection of navigation items.</returns>
    IEnumerable<INavigationItemViewModel> GetNavigationItems();
}

internal sealed class NavigationItemsRegistry : INavigationItemsRegistry
{
    private readonly IServiceProvider serviceProvider;
    private readonly HashSet<Type> navigationItemTypes = [];

    // Initialize navigation items from options
    public NavigationItemsRegistry(IServiceProvider serviceProvider, IOptions<NavigationItemsRegistryOptions> options)
    {
        this.serviceProvider = serviceProvider;

        foreach (var type in options.Value.NavigationItemTypes)
        {
            RegisterNavigationItem(type);
        }
    }

    public void RegisterNavigationItem(Type navigationItemType)
    {
        ArgumentNullException.ThrowIfNull(navigationItemType);

        if (!typeof(INavigationItemViewModel).IsAssignableFrom(navigationItemType))
        {
            throw new ArgumentException(
                $"Type '{navigationItemType.Name}' does not implement INavigationItemViewModel.",
                nameof(navigationItemType)
            );
        }

        navigationItemTypes.Add(navigationItemType);
    }

    public IEnumerable<INavigationItemViewModel> GetNavigationItems()
    {
        foreach (var type in navigationItemTypes)
        {
            if (serviceProvider.GetService(type) is INavigationItemViewModel navigationItem)
            {
                yield return navigationItem;
            }
        }
    }
}
