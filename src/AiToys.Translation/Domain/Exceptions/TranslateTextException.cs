namespace AiToys.Translation.Domain.Exceptions;

/// <summary>
/// Exception thrown when an error occurs during text translation.
/// </summary>
[Serializable]
public class TranslateTextException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateTextException"/> class with a default error message.
    /// </summary>
    public TranslateTextException()
        : base("An error occurred while translating the text.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateTextException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public TranslateTextException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateTextException"/> class with a default error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public TranslateTextException(Exception innerException)
        : base("An error occurred while translating the text.", innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateTextException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TranslateTextException(string message, Exception innerException)
        : base(message, innerException) { }
}
