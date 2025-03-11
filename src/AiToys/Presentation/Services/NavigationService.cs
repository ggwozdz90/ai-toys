using AiToys.Presentation.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace AiToys.Presentation.Services;

internal sealed class NavigationService(IServiceProvider serviceProvider, IViewResolver viewResolver)
    : INavigationService
{
    private Frame? navigationFrame;

    public void SetNavigationFrame(Frame frame)
    {
        navigationFrame = frame ?? throw new ArgumentNullException(nameof(frame));
    }

    public void Navigate<TViewModel>()
        where TViewModel : IViewModel
    {
        if (navigationFrame is null)
        {
            throw new InvalidOperationException("Navigation frame is not set.");
        }

        var viewType = viewResolver.ResolveViewType<TViewModel>();

        var view = ActivatorUtilities.CreateInstance(serviceProvider, viewType);

        navigationFrame.Content = view;
    }
}
