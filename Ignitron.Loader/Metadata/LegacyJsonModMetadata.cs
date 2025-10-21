using System.Text.Json.Serialization;

namespace Ignitron.Loader.Metadata;

internal sealed class LegacyJsonModMetadata : IModMetadata, IJsonOnDeserialized
{
    [JsonInclude] private string? Author { get; set; }
    [JsonInclude] [JsonPropertyName("Dependencies")] private LegacyJsonModDependency[] LegacyDependencies { get; set; } 
    
    [JsonPropertyName("AssemblyFile")] [JsonRequired] public string AssemblyRelativePath { get; set; }
    [JsonRequired] public string Id { get; set; }
    [JsonPropertyName("Name")] [JsonRequired] public string DisplayName { get; set; }
    [JsonIgnore] public IEnumerable<ModContributor> Contributors { get; private set; } = [];
    public string? Description { get; set; }
    [JsonRequired] public Version Version { get; set; }
    [JsonIgnore] public IEnumerable<IModDependency> Dependencies => LegacyDependencies;
    [JsonIgnore] public IEnumerable<string>? Entrypoints => null;

    void IJsonOnDeserialized.OnDeserialized()
    {
        // convert 'Author' field to 'Contributors'
        if (Author != null)
        {
            Contributors = [new ModContributor(Author, "Author")];
        }
    }
}