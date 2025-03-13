using AiToys.Presentation.ViewModels;
using AiToys.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys.Extensions;

internal static class AppHostBuilderExtensions
{
    public static IHostBuilder ConfigureApp(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (_, services) =>
            {
                services.AddSingleton<MainViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<HomePage>();
            }
        );

        return hostBuilder;
    }
}
