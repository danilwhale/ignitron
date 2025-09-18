namespace Ignitron.Loader.API;

public interface IProgressDisplay
{
    void UpdateCategory(string category);
    void UpdateMessage(string message);
}