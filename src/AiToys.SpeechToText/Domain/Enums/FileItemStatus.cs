namespace AiToys.SpeechToText.Domain.Enums;

/// <summary>
/// Represents the status of a file item.
/// </summary>
internal enum FileItemStatus
{
    /// <summary>
    /// The file item has been added to the list.
    /// </summary>
    Added,

    /// <summary>
    /// The file item is pending processing.
    /// </summary>
    Pending,

    /// <summary>
    /// The file item is currently being processed.
    /// </summary>
    Processing,

    /// <summary>
    /// The file item has been successfully processed.
    /// </summary>
    Completed,

    /// <summary>
    /// The file item processing has failed.
    /// </summary>
    Failed,
}
