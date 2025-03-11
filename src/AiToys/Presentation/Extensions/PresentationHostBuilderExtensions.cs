using AiToys.Presentation.Contracts;
using AiToys.Presentation.Services;
using AiToys.Presentation.ViewModels;
using AiToys.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys.Presentation.Extensions;

internal static class PresentationHostBuilderExtensions
{
    public static IHostBuilder ConfigurePresentationLayer(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (_, services) =>
            {
                services.AddSingleton<IViewResolver, ViewResolver>(_ =>
                {
                    var viewResolver = new ViewResolver();

                    RegisterViewMappings(viewResolver);

                    return viewResolver;
                });

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<INavigationService, NavigationService>();

                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<HomePage>();
            }
        );

        return hostBuilder;
    }

    private static void RegisterViewMappings(ViewResolver viewResolver)
    {
        viewResolver.RegisterView<HomePage, HomeViewModel>();
    }
}
