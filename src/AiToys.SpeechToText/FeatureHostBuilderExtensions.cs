using AiToys.Core;
using AiToys.SpeechToText.Application.Services;
using AiToys.SpeechToText.Application.UseCases;
using AiToys.SpeechToText.Constants;
using AiToys.SpeechToText.Data.Adapters;
using AiToys.SpeechToText.Data.Repositories;
using AiToys.SpeechToText.Domain.Adapters;
using AiToys.SpeechToText.Domain.Repositories;
using AiToys.SpeechToText.Presentation.Factories;
using AiToys.SpeechToText.Presentation.ViewModels;
using AiToys.SpeechToText.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpeechToTextApiClient.DependencyInjection;

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
    public static IHostBuilder ConfigureSpeechToTextFeature(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices(
            (context, services) =>
            {
                services.AddScoped<IFilePickerAdapter, FilePickerAdapter>();

                services.AddScoped<ISpeechToTextRepository, SpeechToTextRepository>();
                services.AddScoped<IFileRepository, FileRepository>();
                services.AddScoped<IFileExtensionsRepository, FileExtensionsRepository>();

                services.AddSingleton<IFileStatusNotifierService, FileStatusNotifierService>();
                services.AddSingleton<IFileProcessingQueueService, FileProcessingQueueService>();

                services.AddScoped<ISelectFilesUseCase, SelectFilesUseCase>();
                services.AddScoped<ISelectFolderUseCase, SelectFolderUseCase>();
                services.AddScoped<IGetSupportedLanguagesUseCase, GetSupportedLanguagesUseCase>();
                services.AddScoped<IGetDefaultFileExtensionsUseCase, GetDefaultFileExtensionsUseCase>();
                services.AddScoped<ITranscribeFileUseCase, TranscribeFileUseCase>();
                services.AddScoped<ISaveTranscriptionUseCase, SaveTranscriptionUseCase>();
                services.AddScoped<IHealthCheckUseCase, HealthCheckUseCase>();

                services.AddSingleton<IFileItemViewModelFactory, FileItemViewModelFactory>();

                services.RegisterView<SpeechToTextPage, SpeechToTextViewModel, SpeechToTextNavigationItemViewModel>(
                    RouteNames.SpeechToTextPage
                );

                services.AddSpeechToTextProcessor(context.Configuration);
            }
        );

        return hostBuilder;
    }
}
