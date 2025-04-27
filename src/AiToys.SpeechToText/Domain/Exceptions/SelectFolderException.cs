namespace AiToys.SpeechToText.Domain.Exceptions;

/// <summary>
/// Exception thrown when an error occurs while selecting a folder.
/// </summary>
[Serializable]
public class SelectFolderException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectFolderException"/> class with a default error message.
    /// </summary>
    public SelectFolderException()
        : base("An error occurred while selecting a folder.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectFolderException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public SelectFolderException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectFolderException"/> class with a default error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public SelectFolderException(Exception innerException)
        : base("An error occurred while selecting a folder.", innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectFolderException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public SelectFolderException(string message, Exception innerException)
        : base(message, innerException) { }
}
