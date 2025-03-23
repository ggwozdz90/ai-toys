using System.Collections.ObjectModel;
using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;
using AiToys.Home.Constants;
using AiToys.Home.Presentation.Factories;

namespace AiToys.Home.Presentation.ViewModels;

internal sealed partial class HomeViewModel : ViewModelBase, IRouteAwareViewModel
{
    private readonly INavigationService navigationService;
    private readonly IFeatureTileViewModelFactory featureTileViewModelFactory;

    public HomeViewModel(INavigationService navigationService, IFeatureTileViewModelFactory featureTileViewModelFactory)
    {
        this.navigationService = navigationService;
        this.featureTileViewModelFactory = featureTileViewModelFactory;

        LoadNavigationItems();
    }

    public string Route => RouteNames.HomePage;

    public ObservableCollection<IFeatureTileViewModel> FeatureTiles { get; } = [];

    private void LoadNavigationItems()
    {
        var navigationItems = navigationService
            .GetNavigationItems()
            .Where(item => !string.Equals(item.Route, Route, StringComparison.Ordinal))
            .OrderBy(item => item.Order);

        FeatureTiles.Clear();

        foreach (var navigationItem in navigationItems)
        {
            var featureTileViewModel = featureTileViewModelFactory.Create(navigationItem);
            FeatureTiles.Add(featureTileViewModel);
        }
    }
}
