namespace Ignitron.Loader;

/// <summary>
/// Represents a mod entrypoint
/// </summary>
public interface IModEntrypoint
{
    /// <summary>
    /// The main method of your mod. This method is invoked after Ignitron collects all mods and resolves their dependencies
    /// </summary>
    /// <param name="box">Box with details about the mod</param>
    void Main(ModBox box);
}