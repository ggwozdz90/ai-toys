using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;
using Extensions.Hosting.WinUi;

namespace AiToys.Home.Presentation.ViewModels;

internal interface IFeatureTileViewModel : IDisposable
{
    string Label { get; }
    string Route { get; }
    string IconKey { get; }
    string Description { get; }
    ICommandBase NavigateCommand { get; }
}

internal sealed partial class FeatureTileViewModel(
    IDispatcherService dispatcherService,
    INavigationService navigationService,
    string label,
    string route,
    string iconKey,
    string description
) : ViewModelBase(dispatcherService), IFeatureTileViewModel
{
    public string Label { get; } = label;

    public string Route { get; } = route;

    public string IconKey { get; } = iconKey;

    public string Description { get; } = description;

    public ICommandBase NavigateCommand { get; } =
        new RelayCommand<string>(route =>
        {
            if (!string.IsNullOrEmpty(route))
            {
                navigationService.NavigateTo(route);
            }
        });

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            NavigateCommand.Dispose();
        }

        base.Dispose(disposing);
    }
}
