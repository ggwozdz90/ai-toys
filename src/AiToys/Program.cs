using System.Diagnostics.CodeAnalysis;
using AiToys.Core;
using AiToys.Extensions;
using AiToys.HomeFeature.Extensions;
using AiToys.Presentation.Views;
using Extensions.Hosting.WinUi;
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
            .ConfigureCore()
            .ConfigureApp()
            .ConfigureHomeFeature()
            .ConfigureWinUi<App, MainWindow>()
            .Build();

        host.ConfigureHomeViews();

        host.Run();
    }
}
