using AiToys.Core;
using AiToys.Home.Constants;
using AiToys.Home.Presentation.Factories;
using AiToys.Home.Presentation.ViewModels;
using AiToys.Home.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys.Home;

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

                services.AddTransient<IFeatureTileViewModelFactory, FeatureTileViewModelFactory>();
                services.AddTransient<IFeatureTileViewModel, FeatureTileViewModel>();
            }
        );

        return hostBuilder;
    }
}
