using AiToys.Presentation.Contracts;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml.Media;

namespace AiToys.Presentation.ViewModels;

internal sealed class MainViewModel(INavigationService navigationService) : IViewModel
{
    public bool ExtendsContentIntoTitleBar { get; set; } = true;

    public SystemBackdrop SystemBackdrop { get; set; } = new MicaBackdrop() { Kind = MicaKind.Base };

    public void NavigateTo<TViewModel>()
        where TViewModel : IViewModel
    {
        navigationService.Navigate<TViewModel>();
    }
}
