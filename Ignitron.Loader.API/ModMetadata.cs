using System.Text.Json.Serialization;

namespace Ignitron.Loader.API;

public sealed class ModMetadata
{
    [JsonRequired] public string AssemblyFile { get; set; } // the relative path to the mod assembly (e.g. ModMenu.dll)
    [JsonRequired] public string Id { get; set; } // the unique id of the mod
    [JsonRequired] public string Name { get; set; } // the name of the mod
    public string? Author { get; set; } // optional author/authors of the mod
    [JsonRequired] public Version Version { get; set; } // the version of the mod
    public string? Description { get; set; } // optional description of the mod
}