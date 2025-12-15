using System.Text.Json.Serialization;

namespace Ignitron.Loader.Metadata.Json;

internal sealed class JsonV2ModMetadata : IModMetadata
{
    // junk, too lazy to make jsonconverter for this
    [JsonPropertyName("contributors")] [JsonInclude] [JsonRequired] private JsonV2ModContributor[] _contributors;
    [JsonPropertyName("dependencies")] [JsonInclude] [JsonRequired] private JsonV2ModDependency[] _dependencies;
    
    [JsonPropertyName("assembly_relative_path")] [JsonRequired] public string AssemblyRelativePath { get; set; }
    [JsonPropertyName("id")] [JsonRequired] public string Id { get; set; }
    [JsonPropertyName("display_name")] [JsonRequired] public string DisplayName { get; set; }
    [JsonIgnore] public IEnumerable<IModContributor> Contributors => _contributors;
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("version")] [JsonRequired] public Version Version { get; set; }
    [JsonIgnore] public IEnumerable<IModDependency> Dependencies => _dependencies;
    [JsonPropertyName("entrypoints")] public IEnumerable<string>? Entrypoints { get; set; }
}