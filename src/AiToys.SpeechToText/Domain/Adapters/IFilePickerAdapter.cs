namespace AiToys.SpeechToText.Domain.Adapters;

/// <summary>
/// Interface for an adapter that handles file and folder selection dialogs.
/// </summary>
public interface IFilePickerAdapter
{
    /// <summary>
    /// Shows a file open dialog and returns the selected file paths.
    /// </summary>
    /// <returns>A list of selected file paths or empty list if canceled.</returns>
    Task<IReadOnlyList<string>> ShowOpenFileDialogAsync();

    /// <summary>
    /// Shows a folder open dialog and returns the selected folder path.
    /// </summary>
    /// <returns>The selected folder path or null if canceled.</returns>
    Task<string?> ShowOpenFolderDialogAsync();
}
