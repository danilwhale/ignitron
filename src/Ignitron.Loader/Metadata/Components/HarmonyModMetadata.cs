using HarmonyLib;

namespace Ignitron.Loader.Metadata.Components;

internal sealed class HarmonyModMetadata : IModMetadata
{
    public string AssemblyRelativePath => "0Harmony.dll";
    public string Id => "harmony";
    public string DisplayName => "Harmony 2";
    public IEnumerable<IModContributor> Contributors { get; } = [new ModContributor("pardeike", "developer")];
    public string Description => "A library for patching, replacing and decorating .NET and Mono methods during runtime.";
    public Version Version { get; }
    public IEnumerable<IModDependency> Dependencies { get; } = [];
    public IEnumerable<string> Entrypoints { get; } = [];

    public HarmonyModMetadata()
    {
        Harmony.VersionInfo(out Version installedVersion);
        Version = installedVersion;
    }
}