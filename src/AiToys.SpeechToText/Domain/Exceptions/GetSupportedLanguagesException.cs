namespace AiToys.SpeechToText.Domain.Exceptions;

/// <summary>
/// Exception thrown when an error occurs while retrieving supported languages.
/// </summary>
[Serializable]
public class GetSupportedLanguagesException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSupportedLanguagesException"/> class with a default error message.
    /// </summary>
    public GetSupportedLanguagesException()
        : base("An error occurred while retrieving supported languages.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSupportedLanguagesException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public GetSupportedLanguagesException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSupportedLanguagesException"/> class with a default error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public GetSupportedLanguagesException(Exception innerException)
        : base("An error occurred while retrieving supported languages.", innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSupportedLanguagesException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public GetSupportedLanguagesException(string message, Exception innerException)
        : base(message, innerException) { }
}
