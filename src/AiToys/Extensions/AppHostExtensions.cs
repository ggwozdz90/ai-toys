using AiToys.Core.Presentation.Contracts;
using AiToys.Presentation.ViewModels;
using AiToys.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys.Extensions;

internal static class AppHostExtensions
{
    public static IHost ConfigureViews(this IHost host)
    {
        ArgumentNullException.ThrowIfNull(host);

        var viewResolver = host.Services.GetRequiredService<IViewResolver>();

        RegisterApplicationViews(viewResolver);

        return host;
    }

    private static void RegisterApplicationViews(IViewResolver viewResolver)
    {
        ArgumentNullException.ThrowIfNull(viewResolver);

        viewResolver.RegisterView<HomePage, HomeViewModel>();
    }
}
