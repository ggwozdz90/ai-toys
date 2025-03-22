using AiToys.Core.Presentation.Extensions;
using AiToys.Core.Presentation.Services;
using AiToys.HomeFeature.Constants;
using AiToys.HomeFeature.Presentation.Services;
using AiToys.HomeFeature.Presentation.ViewModels;
using AiToys.HomeFeature.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys.HomeFeature.Extensions;

/// <summary>
/// Extension methods for configuring the feature.
/// </summary>
public static class FeatureHostBuilderExtensions
{
    /// <summary>
    /// Configures the services.
    /// </summary>
    /// <param name="hostBuilder">The host builder.</param>
    /// <returns>The configured host builder with services registered.</returns>
    public static IHostBuilder ConfigureHomeFeature(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (_, services) =>
            {
                services.RegisterView<HomePage, HomeViewModel, HomeNavigationItemViewModel>(RouteNames.HomePage);

                services.AddTransient<INavigationItemsProvider, FeatureNavigationItemsProvider>();
            }
        );

        return hostBuilder;
    }
}
