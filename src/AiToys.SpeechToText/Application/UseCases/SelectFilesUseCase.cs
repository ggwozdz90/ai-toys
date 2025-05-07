using System.IO.Abstractions;
using AiToys.SpeechToText.Domain.Adapters;
using AiToys.SpeechToText.Domain.Exceptions;
using AiToys.SpeechToText.Domain.Models;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Application.UseCases;

internal interface ISelectFilesUseCase
{
    Task<IReadOnlyList<FileItemModel>> ExecuteAsync(
        string fileExtensions,
        CancellationToken cancellationToken = default
    );
}

internal sealed class SelectFilesUseCase(
    IFilePickerAdapter filePickerAdapter,
    IFileSystem fileSystem,
    ILogger<SelectFilesUseCase> logger
) : ISelectFilesUseCase
{
    public async Task<IReadOnlyList<FileItemModel>> ExecuteAsync(
        string fileExtensions,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation("Opening file selection dialog");

        try
        {
            var filePaths = await filePickerAdapter.ShowOpenFileDialogAsync().ConfigureAwait(false);

            logger.LogInformation("Selected {Count} files", filePaths.Count);

            var fileItems = filePaths.Select(filePath => new FileItemModel(filePath, fileSystem)).ToList();

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
            logger.LogError(ex, "Error selecting files: {ErrorMessage}", ex.Message);
            throw new SelectFilesException(ex);
        }
    }
}
