using AiToys.AudioFeature.Presentation.ViewModels;
using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AiToys.AudioFeature.Presentation.Services;

internal sealed class FeatureNavigationItemsProvider(IServiceProvider serviceProvider) : INavigationItemsProvider
{
    public IEnumerable<INavigationItemViewModel> GetNavigationItems()
    {
        yield return serviceProvider.GetRequiredService<SpeechToTextNavigationItemViewModel>();
    }
}
