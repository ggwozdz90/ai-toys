using System.Windows.Input;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.Services;
using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Presentation.ViewModels;

internal sealed partial class AppTitleBarViewModel(INavigationService navigationService) : ViewModelBase
{
    private ICommand? goBackCommand;

    public ICommand GoBackCommand => goBackCommand ??= new RelayCommand(GoBack);

    public void GoBack()
    {
        navigationService.GoBack();
    }
}
