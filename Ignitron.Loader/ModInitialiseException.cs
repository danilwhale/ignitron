namespace Ignitron.Loader;

public sealed class ModInitialiseException : Exception
{
    public ModInitialiseException(string? message)
        : base(message)
    {
    }

    public ModInitialiseException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}