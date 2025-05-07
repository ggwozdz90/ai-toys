namespace AiToys.SpeechToText.Domain.Exceptions;

/// <summary>
/// Exception thrown when an error occurs while saving the transcription to a file.
/// </summary>
[Serializable]
public class SaveTranscriptionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveTranscriptionException"/> class with a default error message.
    /// </summary>
    public SaveTranscriptionException()
        : base("An error occurred while saving the transcription file.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveTranscriptionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public SaveTranscriptionException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveTranscriptionException"/> class with a default error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public SaveTranscriptionException(Exception innerException)
        : base("An error occurred while saving the transcription file.", innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveTranscriptionException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public SaveTranscriptionException(string message, Exception innerException)
        : base(message, innerException) { }
}
