using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace Extensions.Hosting.WinUi;

/// <summary>
/// Provides extension methods for configuring WinUI with the .NET Generic Host.
/// </summary>
#pragma warning disable CA1515
public static class WinUiHostBuilderExtensions
#pragma warning restore CA1515
{
    /// <summary>
    /// Configures the WinUI application and its main window type with the .NET Generic Host.
    /// This method ensures that the necessary WinUI services are registered as singletons
    /// and that the application and window types are properly configured.
    /// </summary>
    /// <typeparam name="TApplication">The type of the WinUI application.</typeparam>
    /// <typeparam name="TWindow">The type of the main window of the WinUI application.</typeparam>
    /// <param name="hostBuilder">The host builder to configure.</param>
    /// <returns>The configured host builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="hostBuilder"/> is null.</exception>
    public static IHostBuilder ConfigureWinUi<TApplication, TWindow>(this IHostBuilder hostBuilder)
        where TApplication : Application
        where TWindow : Window
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (_, services) =>
            {
                var winUiContext = new WinUiContext { MainWindowType = typeof(TWindow), IsLifetimeLinked = true };

                services.AddSingleton(winUiContext);
                services.AddSingleton(serviceProvider => new WinUiThread(
                    serviceProvider,
                    winUiContext,
                    serviceProvider.GetRequiredService<IHostApplicationLifetime>()
                ));
                services.AddHostedService<WinUiHostedService>();
                services.AddSingleton<TApplication>();
                services.AddSingleton<Application>(serviceProvider =>
                    serviceProvider.GetRequiredService<TApplication>()
                );
                services.AddSingleton<IWindowProvider, WindowProvider>();
            }
        );

        return hostBuilder;
    }
}
