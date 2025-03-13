using AiToys.Core.Presentation.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace AiToys.Core.Presentation.Services;

internal sealed class NavigationService(IServiceProvider serviceProvider, IViewResolver viewResolver)
    : INavigationService
{
    private Frame? navigationFrame;

    public void SetNavigationFrame(Frame frame)
    {
        navigationFrame = frame ?? throw new ArgumentNullException(nameof(frame));
    }

    public void NavigateTo<TViewModel>()
        where TViewModel : IViewModel
    {
        if (navigationFrame is null)
        {
            throw new InvalidOperationException("Navigation frame is not set.");
        }

        var viewType = viewResolver.ResolveViewType<TViewModel>();
        var viewModel = serviceProvider.GetRequiredService<TViewModel>();

        navigationFrame.Navigate(viewType);

        if (navigationFrame.Content is IView<TViewModel> view)
        {
            view.ViewModel = viewModel;
        }
    }
}
