using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Presentation.ViewModels;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Presentation.Factories;

internal interface IFileItemViewModelFactory
{
    FileItemViewModel Create(FileItemModel fileItem);
}

internal sealed class FileItemViewModelFactory(ILoggerFactory loggerFactory) : IFileItemViewModelFactory
{
    public FileItemViewModel Create(FileItemModel fileItem)
    {
        return new FileItemViewModel(fileItem, loggerFactory.CreateLogger<FileItemViewModel>());
    }
}
