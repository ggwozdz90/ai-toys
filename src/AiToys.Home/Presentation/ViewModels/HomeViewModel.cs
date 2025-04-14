using System.Collections.ObjectModel;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;
using AiToys.Home.Constants;
using AiToys.Home.Presentation.Factories;

namespace AiToys.Home.Presentation.ViewModels;

internal sealed partial class HomeViewModel : ViewModelBase, IRouteAwareViewModel, IInitializableViewModel
{
    private readonly INavigationService navigationService;
    private readonly IFeatureTileViewModelFactory featureTileViewModelFactory;

    public HomeViewModel(INavigationService navigationService, IFeatureTileViewModelFactory featureTileViewModelFactory)
    {
        this.navigationService = navigationService;
        this.featureTileViewModelFactory = featureTileViewModelFactory;

        InitializeCommand = new RelayCommand(ExecuteInitialize);
    }

    public string Route => RouteNames.HomePage;

    public ObservableCollection<IFeatureTileViewModel> FeatureTiles { get; } = [];

    public ICommandBase InitializeCommand { get; }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var featureTile in FeatureTiles)
            {
                featureTile.Dispose();
            }

            InitializeCommand.Dispose();
        }

        base.Dispose(disposing);
    }

    private void ExecuteInitialize()
    {
        var navigationItems = navigationService
            .GetNavigationItems()
            .Where(item => !string.Equals(item.Route, Route, StringComparison.Ordinal))
            .OrderBy(item => item.Order);

        foreach (var tile in FeatureTiles)
        {
            tile.Dispose();
        }

        FeatureTiles.Clear();

        foreach (var navigationItem in navigationItems)
        {
            var featureTileViewModel = featureTileViewModelFactory.Create(navigationItem);
            FeatureTiles.Add(featureTileViewModel);
        }
    }
}
