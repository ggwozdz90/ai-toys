using AiToys.SpeechToText.Domain.Adapters;
using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Models;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Application.UseCases;

internal interface ISelectFilesUseCase
{
    Task<IReadOnlyList<FileItemModel>> ExecuteAsync(CancellationToken cancellationToken = default);
}

internal sealed class SelectFilesUseCase(IFilePickerAdapter filePickerAdapter, ILogger<SelectFilesUseCase> logger)
    : ISelectFilesUseCase
{
    public async Task<IReadOnlyList<FileItemModel>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Opening file selection dialog");

        try
        {
            var filePaths = await filePickerAdapter.ShowOpenFileDialogAsync().ConfigureAwait(false);

            logger.LogInformation("Selected {Count} files", filePaths.Count);

            var fileItems = filePaths.Select(filePath => new FileItemModel(filePath)).ToList();

            return fileItems;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error selecting files: {ErrorMessage}", ex.Message);
            throw new SelectFilesException(ex);
        }
    }
}
