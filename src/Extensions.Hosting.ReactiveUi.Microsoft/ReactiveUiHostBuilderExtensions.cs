using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace Extensions.Hosting.ReactiveUi.Microsoft;

/// <summary>
/// Provides extension methods for configuring ReactiveUI with the Microsoft Dependency Injection.
/// </summary>
#pragma warning disable CA1515
public static class ReactiveUiHostBuilderExtensions
#pragma warning restore CA1515
{
    /// <summary>
    /// Configures the ReactiveUI services with the Microsoft Dependency Injection.
    /// </summary>
    /// <param name="hostBuilder">The host builder to configure.</param>
    /// <returns>The configured host builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="hostBuilder"/> is null.</exception>
    public static IHostBuilder ConfigureReactiveUiForMicrosoftDependencyResolver(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (_, services) =>
            {
                services.UseMicrosoftDependencyResolver();

                var resolver = Locator.CurrentMutable;
                resolver.InitializeSplat();
                resolver.InitializeReactiveUI();
            }
        );

        return hostBuilder;
    }
}
