namespace Ignitron.Loader.Metadata;

internal sealed class AllumeriaModMetadata : IModMetadata
{
    public string AssemblyRelativePath => "Allumeria.dll"; // NOTE: this will be correct as of 0.10
    public string Id => "allumeria";
    public string DisplayName => "Allumeria";
    public IEnumerable<IModContributor> Contributors { get; } = [new ModContributor("unomelon", "Developer")];
    public string? Description => null;
    public Version Version => IgnitronLoader.Instance.GameVersion;
    public IEnumerable<IModDependency> Dependencies { get; } = [];
    public IEnumerable<string>? Entrypoints { get; } = [];
}