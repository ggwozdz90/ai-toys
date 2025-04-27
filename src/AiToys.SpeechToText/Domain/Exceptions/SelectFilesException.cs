namespace AiToys.SpeechToText.Domain.Exceptions;

/// <summary>
/// Exception thrown when an error occurs while selecting files.
/// </summary>
[Serializable]
public class SelectFilesException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectFilesException"/> class with a default error message.
    /// </summary>
    public SelectFilesException()
        : base("An error occurred while selecting files.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectFilesException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public SelectFilesException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectFilesException"/> class with a default error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public SelectFilesException(Exception innerException)
        : base("An error occurred while selecting files.", innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectFilesException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public SelectFilesException(string message, Exception innerException)
        : base(message, innerException) { }
}
