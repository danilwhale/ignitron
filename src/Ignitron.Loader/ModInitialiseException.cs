namespace Ignitron.Loader;

/// <summary>
/// The exception that is thrown when error during mod initialisation occurs
/// </summary>
public sealed class ModInitialiseException : Exception
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ModInitialiseException"/> with a specified error message
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ModInitialiseException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="ModInitialiseException"/>with a specified error message and a reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
    public ModInitialiseException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}