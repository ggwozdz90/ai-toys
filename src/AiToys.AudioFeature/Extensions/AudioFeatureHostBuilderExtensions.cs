using AiToys.AudioFeature.Presentation.Services;
using AiToys.AudioFeature.Presentation.ViewModels;
using AiToys.AudioFeature.Presentation.Views;
using AiToys.Core.Presentation.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys.AudioFeature.Extensions;

/// <summary>
/// Extension methods for configuring the audio feature.
/// </summary>
public static class AudioFeatureHostBuilderExtensions
{
    /// <summary>
    /// Configures the audio services.
    /// </summary>
    /// <param name="hostBuilder">The host builder.</param>
    /// <returns>The configured host builder with audio services registered.</returns>
    public static IHostBuilder ConfigureAudioFeature(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (_, services) =>
            {
                services.AddTransient<SpeechToTextViewModel>();
                services.AddTransient<SpeechToTextPage>();

                services.AddTransient<SpeechToTextNavigationItemViewModel>();
                services.AddTransient<INavigationItemsProvider, AudioFeatureNavigationItemsProvider>();
            }
        );

        return hostBuilder;
    }
}
