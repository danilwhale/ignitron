namespace Ignitron.Loader.Metadata;

/// <summary>
/// Represents a mod dependency
/// </summary>
/// <param name="id">Unique mod ID of a dependency</param>
/// <param name="version">Version of a dependency specified using wildcard</param>
/// <param name="type">Type of dependency</param>
public sealed class ModDependency(string id, WildcardVersion version, ModDependencyType type) : IModDependency
{
    public string Id { get; } = id;
    public WildcardVersion Version { get; } = version;
    public ModDependencyType Type { get; } = type;
}