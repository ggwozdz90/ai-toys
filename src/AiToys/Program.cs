using System.Diagnostics.CodeAnalysis;
using AiToys.Presentation.ViewModels;
using AiToys.Presentation.Views;
using Extensions.Hosting.ReactiveUi.Microsoft;
using Extensions.Hosting.WinUi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys;

/// <summary>
/// The main entry point for the application.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureReactiveUiForMicrosoftDependencyResolver()
            .ConfigureWinUi<App, MainWindow, MainViewModel>()
            .ConfigureViewModels()
            .Build();

        host.Run();
    }

    public static IHostBuilder ConfigureViewModels(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(services =>
        {
            services.AddSingleton<ShellViewModel>();
        });

        return hostBuilder;
    }
}
