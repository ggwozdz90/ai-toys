using System.Diagnostics.CodeAnalysis;
using AiToys.Core;
using AiToys.Extensions;
using AiToys.Home;
using AiToys.Infrastructure.Logging;
using AiToys.Presentation.Views;
using AiToys.SpeechToText;
using AiToys.Translation;
using Extensions.Hosting.WinUi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            .ConfigureAppConfiguration(
                (_, config) => config.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
            )
            .ConfigureLogging(
                (context, logging) =>
                {
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddConsoleLogger(context.Configuration);
                }
            )
            .ConfigureCore()
            .ConfigureApp()
            .ConfigureHomeFeature()
            .ConfigureSpeechToTextFeature()
            .ConfigureTranslationFeature()
            .ConfigureWinUi<App, MainWindow>()
            .Build();

        host.Run();
    }
}
