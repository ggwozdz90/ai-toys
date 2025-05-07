namespace AiToys.SpeechToText.Domain.Exceptions;

/// <summary>
/// Exception thrown when an error occurs while retrieving default file extensions.
/// </summary>
[Serializable]
public class GetDefaultFileExtensionsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetDefaultFileExtensionsException"/> class with a default error message.
    /// </summary>
    public GetDefaultFileExtensionsException()
        : base("An error occurred while retrieving default file extensions.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetDefaultFileExtensionsException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public GetDefaultFileExtensionsException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetDefaultFileExtensionsException"/> class with a default error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public GetDefaultFileExtensionsException(Exception innerException)
        : base("An error occurred while retrieving default file extensions.", innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetDefaultFileExtensionsException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public GetDefaultFileExtensionsException(string message, Exception innerException)
        : base(message, innerException) { }
}
