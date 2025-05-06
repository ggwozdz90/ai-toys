using System.IO.Abstractions;
using AiToys.SpeechToText.Domain.Adapters;
using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Models;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Application.UseCases;

internal interface ISelectFolderUseCase
{
    Task<IList<FileItemModel>> ExecuteAsync(string fileExtensions, CancellationToken cancellationToken = default);
}

internal sealed class SelectFolderUseCase(
    IFilePickerAdapter filePickerAdapter,
    IFileSystem fileSystem,
    ILogger<SelectFolderUseCase> logger
) : ISelectFolderUseCase
{
    public async Task<IList<FileItemModel>> ExecuteAsync(
        string fileExtensions,
        CancellationToken cancellationToken = default
    )
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

            if (!string.IsNullOrWhiteSpace(fileExtensions))
            {
                var extensions = fileExtensions
                    .Split(',')
                    .Select(ext => ext.Trim().ToUpperInvariant())
                    .Where(ext => !string.IsNullOrWhiteSpace(ext))
                    .Select(ext => ext.StartsWith('.') ? ext : $".{ext}")
                    .ToHashSet(StringComparer.Ordinal);

                logger.LogInformation("Filtering files by extensions: {Extensions}", string.Join(", ", extensions));

                fileItems =
                [
                    .. fileItems.Where(file =>
                        extensions.Contains(fileSystem.Path.GetExtension(file.FilePath).ToUpperInvariant())
                    ),
                ];

                logger.LogInformation("{Count} files match the extension filter", fileItems.Count);
            }

            return fileItems;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error selecting folder: {ErrorMessage}", ex.Message);
            throw new SelectFolderException(ex);
        }
    }
}
