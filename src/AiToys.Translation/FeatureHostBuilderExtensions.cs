using AiToys.Core;
using AiToys.Translation.Application.UseCases;
using AiToys.Translation.Constants;
using AiToys.Translation.Data.Repositories;
using AiToys.Translation.Domain.Repositories;
using AiToys.Translation.Presentation.ViewModels;
using AiToys.Translation.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TranslationApiClient.DependencyInjection;

namespace AiToys.Translation;

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
    public static IHostBuilder ConfigureTranslationFeature(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (context, services) =>
            {
                services.AddScoped<ITranslationRepository, TranslationRepository>();

                services.AddScoped<ITranslateTextUseCase, TranslateTextUseCase>();
                services.AddScoped<IGetSupportedLanguagesUseCase, GetSupportedLanguagesUseCase>();

                services.RegisterView<TranslationPage, TranslationViewModel, TranslationNavigationItemViewModel>(
                    RouteNames.TranslationPage
                );

                services.AddTranslationProcessor(context.Configuration);
            }
        );

        return hostBuilder;
    }
}
