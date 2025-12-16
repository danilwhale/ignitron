using System.Text.Json.Serialization;

namespace Ignitron.Loader.Metadata.Json;

internal sealed class JsonV2ModMetadata : IModMetadata
{
    // junk, too lazy to make jsonconverter for this
    [JsonPropertyName("contributors")] [JsonInclude] [JsonRequired] private JsonV2ModContributor[] _contributors = null!;
    [JsonPropertyName("dependencies")] [JsonInclude] [JsonRequired] private JsonV2ModDependency[] _dependencies = null!;
    
    [JsonPropertyName("assembly_relative_path")] [JsonRequired] public string AssemblyRelativePath { get; set; } = null!;
    [JsonPropertyName("id")] [JsonRequired] public string Id { get; set; } = null!;
    [JsonPropertyName("display_name")] [JsonRequired] public string DisplayName { get; set; } = null!;
    [JsonIgnore] public IEnumerable<IModContributor> Contributors => _contributors;
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("version")] [JsonRequired] public Version Version { get; set; } = null!;
    [JsonIgnore] public IEnumerable<IModDependency> Dependencies => _dependencies;
    [JsonPropertyName("entrypoints")] public IEnumerable<string>? Entrypoints { get; set; }
}