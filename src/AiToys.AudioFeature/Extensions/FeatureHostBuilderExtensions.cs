using AiToys.AudioFeature.Constants;
using AiToys.AudioFeature.Presentation.Services;
using AiToys.AudioFeature.Presentation.ViewModels;
using AiToys.AudioFeature.Presentation.Views;
using AiToys.Core;
using AiToys.Core.Presentation.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys.AudioFeature.Extensions;

/// <summary>
/// Extension methods for configuring the feature.
/// </summary>
public static class FeatureHostBuilderExtensions
{
    /// <summary>
    /// Configures the services.
    /// </summary>
    /// <param name="hostBuilder">The host builder.</param>
    /// <returns>The configured host builder with services registered.</returns>
    public static IHostBuilder ConfigureAudioFeature(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (_, services) =>
            {
                services.RegisterView<SpeechToTextPage, SpeechToTextViewModel, SpeechToTextNavigationItemViewModel>(
                    RouteNames.SpeechToTextPage
                );

                services.AddTransient<INavigationItemsProvider, FeatureNavigationItemsProvider>();
            }
        );

        return hostBuilder;
    }
}
