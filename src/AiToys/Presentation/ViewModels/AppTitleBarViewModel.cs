using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Presentation.ViewModels;

internal sealed partial class AppTitleBarViewModel : ViewModelBase
{
    public AppTitleBarViewModel(INavigationService navigationService)
    {
        navigationService.Navigated += (_, _) => GoBackCommand?.NotifyCanExecuteChanged();

        GoBackCommand = new RelayCommand(navigationService.NavigateBack, () => navigationService.CanNavigateBack);
    }

    public IRelayCommand GoBackCommand { get; }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            GoBackCommand.Dispose();
        }

        base.Dispose(disposing);
    }
}
