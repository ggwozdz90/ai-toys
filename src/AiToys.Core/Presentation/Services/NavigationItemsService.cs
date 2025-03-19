using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Core.Presentation.Services;

public class NavigationItemsService(IEnumerable<INavigationItemsProvider> providers) : INavigationItemsService
{
    private readonly IEnumerable<INavigationItemViewModel> navigationItems = providers
        .SelectMany(provider => provider.GetNavigationItems())
        .OrderBy(navigationItemViewModel => navigationItemViewModel.Order)
        .ToList();

    public IReadOnlyList<INavigationItemViewModel> GetNavigationItems() => navigationItems.ToList().AsReadOnly();
}
