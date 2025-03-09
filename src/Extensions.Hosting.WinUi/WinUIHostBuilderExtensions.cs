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
    /// <typeparam name="TViewModel">The type of the view model associated with the main window.</typeparam>
    /// <param name="hostBuilder">The host builder to configure.</param>
    /// <returns>The configured host builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="hostBuilder"/> is null.</exception>
    public static IHostBuilder ConfigureWinUi<TApplication, TWindow, TViewModel>(this IHostBuilder hostBuilder)
        where TApplication : Application
        where TWindow : Window, IWinUiWindow
        where TViewModel : class, IMainViewModel
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (_, services) =>
            {
                var winUiContext = new WinUIContext
                {
                    MainWindowType = typeof(TWindow),
                    MainViewModelType = typeof(TViewModel),
                    IsLifetimeLinked = true,
                };

                services.AddSingleton(winUiContext);
                services.AddSingleton(serviceProvider => new WinUiThread(serviceProvider));
                services.AddHostedService<WinUiHostedService>();
                services.AddSingleton<TApplication>();
                services.AddSingleton<Application>(sp => sp.GetRequiredService<TApplication>());
                services.AddSingleton<TViewModel>();
            }
        );

        return hostBuilder;
    }
}
