namespace Ignitron.Loader.API;

public interface ICrashHandler
{
    void HandleCrash(Exception exception, string? message);
}