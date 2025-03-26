using AiToys.Core.Presentation.Events;
using AiToys.Core.Presentation.ViewModels;
using AiToys.Core.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace AiToys.Core.Presentation.Services;

internal sealed class NavigationService(
    IServiceProvider serviceProvider,
    IViewResolver viewResolver,
    IViewModelResolver viewModelResolver,
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

    private void EnsureNavigationFrameIsSet()
    {
        if (navigationFrame is null)
        {
            throw new InvalidOperationException("Navigation frame is not set.");
        }
    }

    private void SetViewModelOnNavigatedView(Type viewModelType, object viewModel)
    {
        if (navigationFrame!.Content is null)
        {
            return;
        }

        var viewInterfaceType = typeof(IView<>).MakeGenericType(viewModelType);

        if (!viewInterfaceType.IsInstanceOfType(navigationFrame.Content))
        {
            return;
        }

        var viewModelProperty = viewInterfaceType.GetProperty("ViewModel");
        viewModelProperty?.SetValue(navigationFrame.Content, viewModel);
    }

    private void SubscribeNavigationEvents()
    {
        if (navigationFrame != null)
        {
            navigationFrame.Navigating += OnNavigationFrameNavigating;
            navigationFrame.Navigated += OnNavigationFrameNavigated;
            navigationFrame.Navigated += PropagateNavigatedEvent;
        }
    }

    private void UnsubscribeNavigationEvents()
    {
        if (navigationFrame != null)
        {
            navigationFrame.Navigating -= OnNavigationFrameNavigating;
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

        if (viewInterface == null)
        {
            return;
        }

        var viewModelProperty = viewInterface.GetProperty("ViewModel");
        var viewModel = viewModelProperty?.GetValue(content);

        if (viewModel is IRouteAwareViewModel routeAwareViewModel)
        {
            Navigated?.Invoke(this, new NavigatedEventArgs(routeAwareViewModel.Route));
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
        var viewModelType = viewModelResolver.ResolveViewModelType(viewType);
        var viewModel = serviceProvider.GetRequiredService(viewModelType);

        SetViewModelOnNavigatedView(viewModelType, viewModel);
    }

    private void OnNavigationFrameNavigating(object sender, NavigatingCancelEventArgs e)
    {
        if (navigationFrame?.Content == null)
        {
            return;
        }

        var currentView = navigationFrame.Content;

        var viewInterfaces = currentView
            .GetType()
            .GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IView<>))
            .ToList();

        if (viewInterfaces.Count == 0)
        {
            return;
        }

        var viewInterface = viewInterfaces[0];
        var viewModelProperty = viewInterface.GetProperty(nameof(IView<IViewModel>.ViewModel));

        if (viewModelProperty == null)
        {
            return;
        }

        var viewModel = viewModelProperty.GetValue(currentView);

        if (viewModel is IDisposable disposableViewModel)
        {
            disposableViewModel.Dispose();
        }
    }
}
