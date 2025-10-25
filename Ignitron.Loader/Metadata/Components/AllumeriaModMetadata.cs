namespace Ignitron.Loader.Metadata.Components;

internal sealed class AllumeriaModMetadata : IModMetadata
{
    public string AssemblyRelativePath => "Allumeria.dll";
    public string Id => "allumeria";
    public string DisplayName => "Allumeria";
    public IEnumerable<IModContributor> Contributors { get; } = [new ModContributor("unomelon", "Developer")];
    public string? Description => null;
    public Version Version => IgnitronLoader.Instance.GameVersion;
    public IEnumerable<IModDependency> Dependencies { get; } = [];
    public IEnumerable<string> Entrypoints { get; } = [];
}