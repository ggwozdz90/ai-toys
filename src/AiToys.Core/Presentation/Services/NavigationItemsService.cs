using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Core.Presentation.Services;

internal sealed class NavigationItemsService(IEnumerable<INavigationItemsProvider> providers) : INavigationItemsService
{
    private readonly List<INavigationItemViewModel> navigationItems =
    [
        .. providers
            .SelectMany(provider => provider.GetNavigationItems())
            .OrderBy(navigationItem => navigationItem.Order),
    ];

    public IReadOnlyList<INavigationItemViewModel> GetNavigationItems() => navigationItems;
}
