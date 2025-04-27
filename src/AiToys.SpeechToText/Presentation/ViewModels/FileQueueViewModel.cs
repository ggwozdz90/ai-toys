using System.Collections.ObjectModel;
using System.Collections.Specialized;
using AiToys.Core.Presentation.ViewModels;
using AiToys.SpeechToText.Domain.Models;
using AiToys.SpeechToText.Presentation.EventArgs;
using AiToys.SpeechToText.Presentation.Factories;
using Microsoft.Extensions.Logging;

namespace AiToys.SpeechToText.Presentation.ViewModels;

internal sealed partial class FileQueueViewModel : ViewModelBase
{
    private readonly ILogger<FileQueueViewModel> logger;
    private readonly IFileItemViewModelFactory fileItemViewModelFactory;
    private bool hasFiles;

    public FileQueueViewModel(ILogger<FileQueueViewModel> logger, IFileItemViewModelFactory fileItemViewModelFactory)
    {
        this.logger = logger;
        this.fileItemViewModelFactory = fileItemViewModelFactory;

        Files.CollectionChanged += Files_CollectionChanged;

        UpdateHasFiles();
    }

    public ObservableCollection<FileItemViewModel> Files { get; } = [];

    public bool HasFiles
    {
        get => hasFiles;
        private set => SetProperty(ref hasFiles, value);
    }

    public void AddFile(FileItemModel fileItem)
    {
        logger.LogInformation("Adding file to queue: {FilePath}", fileItem.FilePath);

        var fileItemViewModel = fileItemViewModelFactory.Create(fileItem);
        fileItemViewModel.RemoveRequested += OnFileRemoveRequested;

        ExecuteOnUIThread(() => Files.Add(fileItemViewModel));
    }

    public void AddFiles(IEnumerable<FileItemModel> fileItems)
    {
        var fileItemsToAdd = fileItems.ToList();

        if (fileItemsToAdd.Count == 0)
        {
            return;
        }

        foreach (var fileItem in fileItemsToAdd)
        {
            AddFile(fileItem);
        }

        logger.LogInformation("Added {Count} files to the queue", fileItemsToAdd.Count);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Files.CollectionChanged -= Files_CollectionChanged;

            foreach (var file in Files)
            {
                file.RemoveRequested -= OnFileRemoveRequested;
                file.Dispose();
            }

            ExecuteOnUIThread(() => Files.Clear());

            logger.LogInformation("FileQueueViewModel is being disposed");
        }

        base.Dispose(disposing);
    }

    private void Files_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateHasFiles();
    }

    private void UpdateHasFiles()
    {
        HasFiles = Files.Count > 0;
    }

    private void OnFileRemoveRequested(object? sender, FileItemEventArgs eventArgs)
    {
        var fileItem = eventArgs.FileItem;

        if (Files.Contains(fileItem))
        {
            logger.LogInformation("Removing file from queue: {FilePath}", fileItem.FilePath);

            fileItem.RemoveRequested -= OnFileRemoveRequested;

            ExecuteOnUIThread(() => Files.Remove(fileItem));
        }
    }
}
