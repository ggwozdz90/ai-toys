using AiToys.Core.Presentation.ViewModels;
using AiToys.Core.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace AiToys.Core.Presentation.Services;

internal sealed class NavigationService(IServiceProvider serviceProvider, IViewResolver viewResolver)
    : INavigationService,
        INavigationFrameProvider
{
    private Frame? navigationFrame;

    public bool CanGoBack => navigationFrame?.CanGoBack ?? false;

    public void SetNavigationFrame(Frame frame)
    {
        UnsubscribeNavigationEvents();
        navigationFrame = frame ?? throw new ArgumentNullException(nameof(frame));
        SubscribeNavigationEvents();
    }

    public void NavigateTo<TViewModel>()
        where TViewModel : IViewModel
    {
        EnsureNavigationFrameIsSet();

        var viewType = viewResolver.ResolveViewType<TViewModel>();
        var viewModel = serviceProvider.GetRequiredService<TViewModel>();

        navigationFrame!.Navigate(viewType);
        SetViewModelOnNavigatedView(typeof(TViewModel), viewModel);
    }

    public void NavigateToRoute(string route)
    {
        EnsureNavigationFrameIsSet();

        var (viewType, viewModelType) = viewResolver.ResolveRoute(route);
        var viewModel = serviceProvider.GetRequiredService(viewModelType);

        navigationFrame!.Navigate(viewType);
        SetViewModelOnNavigatedView(viewModelType, viewModel);
    }

    public void GoBack()
    {
        EnsureNavigationFrameIsSet();

        if (CanGoBack)
        {
            navigationFrame!.GoBack();
        }
    }

    private static Type ResolveViewModelType(Type viewType)
    {
        foreach (var interfaceType in viewType.GetInterfaces())
        {
            if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IView<>))
            {
                return interfaceType.GetGenericArguments()[0];
            }
        }

        throw new InvalidOperationException($"View '{viewType}' does not implement IView<TViewModel>.");
    }

    private void EnsureNavigationFrameIsSet()
    {
        if (navigationFrame is null)
        {
            throw new InvalidOperationException("Navigation frame is not set.");
        }
    }

    private void SetViewModelOnNavigatedView(Type viewModelType, object viewModel)
    {
        if (navigationFrame!.Content is not null)
        {
            var viewInterfaceType = typeof(IView<>).MakeGenericType(viewModelType);

            if (viewInterfaceType.IsInstanceOfType(navigationFrame.Content))
            {
                var viewModelProperty = viewInterfaceType.GetProperty("ViewModel");
                viewModelProperty?.SetValue(navigationFrame.Content, viewModel);
            }
        }
    }

    private void SubscribeNavigationEvents()
    {
        if (navigationFrame != null)
        {
            navigationFrame.Navigated += OnNavigationFrameNavigated;
        }
    }

    private void UnsubscribeNavigationEvents()
    {
        if (navigationFrame != null)
        {
            navigationFrame.Navigated -= OnNavigationFrameNavigated;
        }
    }

    private void OnNavigationFrameNavigated(object sender, NavigationEventArgs e)
    {
        if (e.NavigationMode == NavigationMode.Back && navigationFrame?.Content != null)
        {
            EnsureNavigationFrameIsSet();

            var viewType = navigationFrame.Content.GetType();
            var viewModelType = ResolveViewModelType(viewType);

            var viewModel = serviceProvider.GetRequiredService(viewModelType);
            SetViewModelOnNavigatedView(viewModelType, viewModel);
        }
    }
}
