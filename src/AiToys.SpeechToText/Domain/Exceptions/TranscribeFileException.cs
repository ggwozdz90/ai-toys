namespace AiToys.SpeechToText.Domain.Exceptions;

/// <summary>
/// Exception thrown when an error occurs during file transcription.
/// </summary>
[Serializable]
public class TranscribeFileException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TranscribeFileException"/> class with a default error message.
    /// </summary>
    public TranscribeFileException()
        : base("An error occurred while transcribing the file.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TranscribeFileException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public TranscribeFileException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TranscribeFileException"/> class with a default error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public TranscribeFileException(Exception innerException)
        : base("An error occurred while transcribing the file.", innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TranscribeFileException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TranscribeFileException(string message, Exception innerException)
        : base(message, innerException) { }
}
