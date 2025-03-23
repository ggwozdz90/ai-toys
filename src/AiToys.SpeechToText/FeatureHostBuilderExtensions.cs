using AiToys.Core;
using AiToys.SpeechToText.Constants;
using AiToys.SpeechToText.Presentation.ViewModels;
using AiToys.SpeechToText.Presentation.Views;
using Microsoft.Extensions.Hosting;

namespace AiToys.SpeechToText;

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
                services.RegisterView<SpeechToTextPage, SpeechToTextViewModel, SpeechToTextNavigationItemViewModel>(
                    RouteNames.SpeechToTextPage
                )
        );

        return hostBuilder;
    }
}
