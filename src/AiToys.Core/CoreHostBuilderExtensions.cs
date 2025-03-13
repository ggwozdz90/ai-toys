using AiToys.Core.Presentation.Contracts;
using AiToys.Core.Presentation.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys.Core;

/// <summary>
/// Extension methods for configuring the core layer.
/// </summary>
public static class CoreHostBuilderExtensions
{
    /// <summary>
    /// Configures the core services.
    /// </summary>
    /// <param name="hostBuilder">The host builder.</param>
    /// <returns>The configured host builder with core services registered.</returns>
    public static IHostBuilder ConfigureCore(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (_, services) =>
            {
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IViewResolver, ViewResolver>();
            }
        );

        return hostBuilder;
    }
}
