namespace Ignitron.Loader.Metadata;

public sealed class ModDependency(string id, WildcardVersion version, ModDependencyType type) : IModDependency
{
    public string Id { get; } = id;
    public WildcardVersion Version { get; } = version;
    public ModDependencyType Type { get; } = type;
}