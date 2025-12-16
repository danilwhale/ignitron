using Ignitron.Aluminium;

namespace Ignitron.Loader.Metadata.Components;

public sealed class AluminiumModMetadata : IModMetadata
{
    public string AssemblyRelativePath => "Ignitron.Aluminium.dll";
    public string Id => "aluminium";
    public string DisplayName => "Aluminium";
    public IEnumerable<IModContributor> Contributors { get; } = [new ModContributor("danilwhale", "developer")];
    public string? Description => "A component of Ignitron to aid in mod development with common hooks and utilities";
    public Version Version => AluminiumLibrary.Version;
    public IEnumerable<IModDependency> Dependencies { get; } = [
        new ModDependency("ignitron", new WildcardVersion(major: 0, minor: 4), ModDependencyType.Mandatory),
        new ModDependency("harmony", new WildcardVersion(major: 2), ModDependencyType.Mandatory)];
    public IEnumerable<string> Entrypoints { get; } = [];
}