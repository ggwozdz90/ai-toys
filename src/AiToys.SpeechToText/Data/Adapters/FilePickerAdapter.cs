using AiToys.SpeechToText.Domain.Adapters;
using Extensions.Hosting.WinUi;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace AiToys.SpeechToText.Data.Adapters;

internal sealed class FilePickerAdapter(IWindowProvider windowProvider) : IFilePickerAdapter
{
    public async Task<IReadOnlyList<string>> ShowOpenFileDialogAsync()
    {
        var filePicker = new FileOpenPicker
        {
            ViewMode = PickerViewMode.List,
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
        };

        filePicker.FileTypeFilter.Add("*");

        InitializeWithWindow.Initialize(filePicker, windowProvider.GetMainWindowHandle());

        var files = await filePicker.PickMultipleFilesAsync().AsTask().ConfigureAwait(false);

        return files?.Select(file => file.Path).ToList() ?? [];
    }

    public async Task<string?> ShowOpenFolderDialogAsync()
    {
        var folderPicker = new FolderPicker
        {
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            CommitButtonText = "Select Folder",
        };

        folderPicker.FileTypeFilter.Add("*");

        InitializeWithWindow.Initialize(folderPicker, windowProvider.GetMainWindowHandle());

        var folder = await folderPicker.PickSingleFolderAsync().AsTask().ConfigureAwait(false);

        return folder?.Path;
    }
}
