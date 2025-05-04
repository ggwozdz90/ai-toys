using System.Collections.ObjectModel;
using AiToys.SpeechToText.Application.Services;
using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Presentation.ViewModels;
using Extensions.Hosting.WinUi;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Presentation.Factories;

internal interface IFileItemViewModelFactory
{
    FileItemViewModel Create(
        FileItemModel fileItem,
        LanguageModel sourceLanguage,
        LanguageModel targetLanguage,
        ObservableCollection<LanguageModel> availableLanguages
    );
}

internal sealed class FileItemViewModelFactory(
    IDispatcherService dispatcherService,
    ILoggerFactory loggerFactory,
    IFileProcessingQueueService fileProcessingQueueService,
    IFileStatusNotifierService fileStatusNotifierService
) : IFileItemViewModelFactory
{
    public FileItemViewModel Create(
        FileItemModel fileItem,
        LanguageModel sourceLanguage,
        LanguageModel targetLanguage,
        ObservableCollection<LanguageModel> availableLanguages
    )
    {
        return new FileItemViewModel(
            dispatcherService,
            loggerFactory.CreateLogger<FileItemViewModel>(),
            fileProcessingQueueService,
            fileStatusNotifierService,
            fileItem,
            sourceLanguage,
            targetLanguage,
            availableLanguages
        );
    }
}
