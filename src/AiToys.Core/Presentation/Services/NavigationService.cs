using AiToys.Core.Presentation.ViewModels;
using AiToys.Core.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace AiToys.Core.Presentation.Services;

internal sealed class NavigationService(
    IServiceProvider serviceProvider,
    IViewResolver viewResolver,
    INavigationItemsRegistry navigationItemsRegistry
) : INavigationService, INavigationFrameProvider
{
    private Frame? navigationFrame;

    public event EventHandler<NavigatedEventArgs>? Navigated;

    public bool CanNavigateBack => navigationFrame?.CanGoBack ?? false;

    public void SetNavigationFrame(Frame frame)
    {
        UnsubscribeNavigationEvents();
        navigationFrame = frame ?? throw new ArgumentNullException(nameof(frame));
        SubscribeNavigationEvents();
    }

    public void NavigateBack()
    {
        EnsureNavigationFrameIsSet();

        if (CanNavigateBack)
        {
            navigationFrame!.GoBack();
        }
    }

    public void NavigateTo(string route)
    {
        EnsureNavigationFrameIsSet();

        var viewType = viewResolver.ResolveRouteView(route);

        navigationFrame!.Navigate(viewType);
    }

    public IReadOnlyList<INavigationItemViewModel> GetNavigationItems() =>
        [.. navigationItemsRegistry.GetNavigationItems()];

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
            navigationFrame.Navigated += PropagateNavigatedEvent;
        }
    }

    private void UnsubscribeNavigationEvents()
    {
        if (navigationFrame != null)
        {
            navigationFrame.Navigated -= OnNavigationFrameNavigated;
            navigationFrame.Navigated -= PropagateNavigatedEvent;
        }
    }

    private void PropagateNavigatedEvent(object sender, NavigationEventArgs e)
    {
        if (navigationFrame?.Content == null)
        {
            return;
        }

        var content = navigationFrame.Content;

        var viewInterface = content
            .GetType()
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IView<>));

        if (viewInterface != null)
        {
            var viewModelProperty = viewInterface.GetProperty("ViewModel");
            var viewModel = viewModelProperty?.GetValue(content);

            if (viewModel is IRouteAwareViewModel routeAwareViewModel)
            {
                Navigated?.Invoke(this, new NavigatedEventArgs(routeAwareViewModel.Route));
            }
        }
    }

    private void OnNavigationFrameNavigated(object sender, NavigationEventArgs e)
    {
        if (navigationFrame?.Content == null)
        {
            return;
        }

        EnsureNavigationFrameIsSet();

        var viewType = navigationFrame.Content.GetType();
        var viewModelType = ResolveViewModelType(viewType);
        var viewModel = serviceProvider.GetRequiredService(viewModelType);

        SetViewModelOnNavigatedView(viewModelType, viewModel);
    }
}
