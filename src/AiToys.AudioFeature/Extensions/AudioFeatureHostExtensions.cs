using AiToys.AudioFeature.Presentation.ViewModels;
using AiToys.AudioFeature.Presentation.Views;
using AiToys.Core.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiToys.AudioFeature.Extensions;

/// <summary>
/// Extension methods for configuring the audio feature views.
/// </summary>
public static class AudioFeatureHostExtensions
{
    /// <summary>
    /// Configures the audio views.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <returns>The configured host with audio views registered.</returns>
    public static IHost ConfigureAudioViews(this IHost host)
    {
        ArgumentNullException.ThrowIfNull(host);

        var viewResolver = host.Services.GetRequiredService<IViewResolver>();

        RegisterFeatureViews(viewResolver);

        return host;
    }

    private static void RegisterFeatureViews(IViewResolver viewResolver)
    {
        ArgumentNullException.ThrowIfNull(viewResolver);

        viewResolver.RegisterView<SpeechToTextPage, SpeechToTextViewModel>();
    }
}
