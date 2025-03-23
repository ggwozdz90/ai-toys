using System.Windows.Input;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Home.Presentation.ViewModels;

internal interface IFeatureTileViewModel
{
    string Label { get; }
    string Route { get; }
    string IconKey { get; }
    string Description { get; }
    ICommand NavigateCommand { get; }
}

internal sealed partial class FeatureTileViewModel(
    INavigationService navigationService,
    string label,
    string route,
    string iconKey,
    string description
) : ViewModelBase, IFeatureTileViewModel
{
    public string Label { get; } = label;

    public string Route { get; } = route;

    public string IconKey { get; } = iconKey;

    public string Description { get; } = description;

    public ICommand NavigateCommand { get; } =
        new RelayCommand<string>(route =>
        {
            if (!string.IsNullOrEmpty(route))
            {
                navigationService.NavigateTo(route);
            }
        });
}
