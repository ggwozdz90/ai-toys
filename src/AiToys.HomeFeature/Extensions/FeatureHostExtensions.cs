using AiToys.Core.Presentation.Views;
using AiToys.HomeFeature.Presentation.ViewModels;
using AiToys.HomeFeature.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys.HomeFeature.Extensions;

/// <summary>
/// Extension methods for configuring the feature views.
/// </summary>
public static class FeatureHostExtensions
{
    /// <summary>
    /// Configures the views.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <returns>The configured host with views registered.</returns>
    public static IHost ConfigureHomeViews(this IHost host)
    {
        ArgumentNullException.ThrowIfNull(host);

        var viewResolver = host.Services.GetRequiredService<IViewResolver>();

        RegisterFeatureViews(viewResolver);

        return host;
    }

    private static void RegisterFeatureViews(IViewResolver viewResolver)
    {
        ArgumentNullException.ThrowIfNull(viewResolver);

        viewResolver.RegisterView<HomePage, HomeViewModel>();
    }
}
