namespace Ignitron.Loader.Metadata;

/// <summary>
/// Represents a mod dependency
/// </summary>
/// <param name="id">Unique mod ID of a dependency</param>
/// <param name="version">Version of a dependency specified using wildcard</param>
/// <param name="type">Type of dependency</param>
public sealed class ModDependency(string id, WildcardVersion version, ModDependencyType type) : IModDependency
{
    /// <inheritdoc />
    public string Id { get; } = id;

    /// <inheritdoc />
    public WildcardVersion Version { get; } = version;

    /// <inheritdoc />
    public ModDependencyType Type { get; } = type;
}