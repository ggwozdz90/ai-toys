using AiToys.Core.Presentation.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace AiToys.Core.Presentation.Services;

internal sealed class NavigationService(IServiceProvider serviceProvider, IViewResolver viewResolver)
    : INavigationService,
        INavigationFrameProvider
{
    private Frame? navigationFrame;

    public void SetNavigationFrame(Frame frame)
    {
        navigationFrame = frame ?? throw new ArgumentNullException(nameof(frame));
    }

    public void NavigateTo<TViewModel>()
        where TViewModel : IViewModel
    {
        EnsureNavigationFrameIsSet();

        var viewType = viewResolver.ResolveViewType<TViewModel>();
        var viewModel = serviceProvider.GetRequiredService<TViewModel>();

        NavigateAndSetViewModel(viewType, typeof(TViewModel), viewModel);
    }

    public void NavigateToRoute(string route)
    {
        EnsureNavigationFrameIsSet();

        var (viewType, viewModelType) = viewResolver.ResolveRoute(route);
        var viewModel = serviceProvider.GetRequiredService(viewModelType);

        NavigateAndSetViewModel(viewType, viewModelType, viewModel);
    }

    private void EnsureNavigationFrameIsSet()
    {
        if (navigationFrame is null)
        {
            throw new InvalidOperationException("Navigation frame is not set.");
        }
    }

    private void NavigateAndSetViewModel(Type viewType, Type viewModelType, object viewModel)
    {
        navigationFrame!.Navigate(viewType);

        if (navigationFrame.Content is not null)
        {
            var viewInterfaceType = typeof(IView<>).MakeGenericType(viewModelType);

            if (viewInterfaceType.IsInstanceOfType(navigationFrame.Content))
            {
                var viewModelProperty = viewInterfaceType.GetProperty("ViewModel");
                viewModelProperty?.SetValue(navigationFrame.Content, viewModel);
            }
        }
    }
}
