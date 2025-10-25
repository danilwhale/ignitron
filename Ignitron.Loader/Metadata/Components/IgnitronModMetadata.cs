namespace Ignitron.Loader.Metadata.Internal;

internal sealed class IgnitronModMetadata : IModMetadata
{
    public string AssemblyRelativePath => "Loader.dll";
    public string Id => "ignitron";
    public string DisplayName => "Ignitron";
    public IEnumerable<IModContributor> Contributors { get; } = [new ModContributor("danilwhale", "developer")];
    public string? Description => "A mod loader for Allumeria";
    public Version Version => IgnitronLoader.Version;
    public IEnumerable<IModDependency> Dependencies { get; } = [];
    public IEnumerable<string> Entrypoints { get; } = [];
}