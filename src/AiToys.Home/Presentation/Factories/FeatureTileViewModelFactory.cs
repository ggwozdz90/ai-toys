using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;
using AiToys.Home.Presentation.ViewModels;
using Extensions.Hosting.WinUi;

namespace AiToys.Home.Presentation.Factories;

internal interface IFeatureTileViewModelFactory
{
    IFeatureTileViewModel Create(INavigationItemViewModel navigationItemViewModel);
}

internal sealed class FeatureTileViewModelFactory(
    IDispatcherService dispatcherService,
    INavigationService navigationService
) : IFeatureTileViewModelFactory
{
    public IFeatureTileViewModel Create(INavigationItemViewModel navigationItemViewModel)
    {
        ArgumentNullException.ThrowIfNull(navigationItemViewModel);

        return new FeatureTileViewModel(
            dispatcherService,
            navigationService,
            navigationItemViewModel.Label,
            navigationItemViewModel.Route,
            navigationItemViewModel.IconKey,
            navigationItemViewModel.Description
        );
    }
}
