using System.IO.Abstractions;
using AiToys.Presentation.ViewModels;
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
                services.AddSingleton<AppTitleBarViewModel>();
                services.AddScoped<IFileSystem, FileSystem>();
            }
        );

        return hostBuilder;
    }
}
