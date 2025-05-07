using AiToys.Core.Presentation.Options;
using AiToys.Core.Presentation.ViewModels;
using AiToys.Core.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AiToys.Core;

/// <summary>
/// Extension methods for view registration.
/// </summary>
public static class ViewRegistrationExtensions
{
    /// <summary>
    /// Registers a view with its associated view model and route.
    /// </summary>
    /// <typeparam name="TView">The view type implementing IView.</typeparam>
    /// <typeparam name="TViewModel">The view model type implementing IViewModel.</typeparam>
    /// <typeparam name="TNavigationItemViewModel">The navigation item type implementing INavigationItemViewModel.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="route">The route to register for this view.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection RegisterView<TView, TViewModel, TNavigationItemViewModel>(
        this IServiceCollection services,
        string route
    )
        where TView : class, IView<TViewModel>
        where TViewModel : class, IViewModel
        where TNavigationItemViewModel : class, INavigationItemViewModel
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrEmpty(route);

        services.TryAddTransient<TView>();
        services.TryAddTransient<TViewModel>();
        services.TryAddSingleton<TNavigationItemViewModel>();

        services.Configure<RouteRegistryOptions>(options => options.RouteMappings[route] = typeof(TView));

        services.Configure<NavigationItemsRegistryOptions>(options =>
            options.NavigationItemTypes.Add(typeof(TNavigationItemViewModel))
        );

        return services;
    }
}
