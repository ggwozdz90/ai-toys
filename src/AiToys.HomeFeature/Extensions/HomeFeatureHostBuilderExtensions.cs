using AiToys.HomeFeature.Presentation.ViewModels;
using AiToys.HomeFeature.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys.HomeFeature.Extensions;

/// <summary>
/// Extension methods for configuring the home feature.
/// </summary>
public static class HomeFeatureHostBuilderExtensions
{
    /// <summary>
    /// Configures the home services.
    /// </summary>
    /// <param name="hostBuilder">The host builder.</param>
    /// <returns>The configured host builder with home services registered.</returns>
    public static IHostBuilder ConfigureHomeFeature(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (_, services) =>
            {
                services.AddTransient<HomeViewModel>();
                services.AddTransient<HomePage>();
            }
        );

        return hostBuilder;
    }
}
