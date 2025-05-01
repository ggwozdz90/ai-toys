using System.IO.Abstractions;
using AiToys.SpeechToText.Domain.Adapters;
using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Models;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Application.UseCases;

internal interface ISelectFolderUseCase
{
    Task<IList<FileItemModel>> ExecuteAsync(CancellationToken cancellationToken = default);
}

internal sealed class SelectFolderUseCase(
    IFilePickerAdapter filePickerAdapter,
    IFileSystem fileSystem,
    ILogger<SelectFolderUseCase> logger
) : ISelectFolderUseCase
{
    public async Task<IList<FileItemModel>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Opening folder selection dialog");

        try
        {
            var folderPath = await filePickerAdapter.ShowOpenFolderDialogAsync().ConfigureAwait(false);

            if (string.IsNullOrEmpty(folderPath))
            {
                logger.LogInformation("Folder selection canceled");
                return [];
            }

            logger.LogInformation("Selected folder: {FolderPath}", folderPath);

            var files = fileSystem.Directory.GetFiles(folderPath);
            var fileItems = files.Select(filePath => new FileItemModel(filePath, fileSystem)).ToList();

            logger.LogInformation("Found {Count} files in the selected folder", fileItems.Count);

            return fileItems;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error selecting folder: {ErrorMessage}", ex.Message);
            throw new SelectFolderException(ex);
        }
    }
}
