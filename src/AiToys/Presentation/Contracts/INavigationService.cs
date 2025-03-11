using Microsoft.UI.Xaml.Controls;

namespace AiToys.Presentation.Contracts;

internal interface INavigationService
{
    void SetNavigationFrame(Frame frame);

    void Navigate<TViewModel>()
        where TViewModel : IViewModel;
}
