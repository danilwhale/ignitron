namespace Ignitron.Loader;

public sealed class ModResolverException : Exception
{
    public ModResolverException(string? message)
        : base(message)
    {
    }

    public ModResolverException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}