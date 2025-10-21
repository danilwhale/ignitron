namespace Ignitron.Loader.Metadata;

public interface IModMetadata
{
    string AssemblyRelativePath { get; }
    string Id { get; }
    string DisplayName { get; }
    IEnumerable<IModContributor> Contributors { get; }
    string? Description { get; }
    Version Version { get; }
    IEnumerable<IModDependency> Dependencies { get; }
    IEnumerable<string> Entrypoints { get; }
}