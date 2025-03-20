using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;
using AiToys.HomeFeature.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AiToys.HomeFeature.Presentation.Services;

internal sealed class FeatureNavigationItemsProvider(IServiceProvider serviceProvider) : INavigationItemsProvider
{
    public IEnumerable<INavigationItemViewModel> GetNavigationItems()
    {
        yield return serviceProvider.GetRequiredService<HomeNavigationItemViewModel>();
    }
}
