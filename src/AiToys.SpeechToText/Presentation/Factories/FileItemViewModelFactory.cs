using System.Collections.ObjectModel;
using AiToys.SpeechToText.Application.UseCases;
using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Presentation.ViewModels;
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
    ILoggerFactory loggerFactory,
    ITranscribeFileUseCase transcribeFileUseCase
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
            loggerFactory.CreateLogger<FileItemViewModel>(),
            transcribeFileUseCase,
            fileItem,
            sourceLanguage,
            targetLanguage,
            availableLanguages
        );
    }
}
