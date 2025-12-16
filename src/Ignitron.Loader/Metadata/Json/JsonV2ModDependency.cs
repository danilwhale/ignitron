using System.Text.Json.Serialization;

namespace Ignitron.Loader.Metadata.Json;

internal sealed class JsonV2ModDependency : IModDependency
{
    [JsonPropertyName("id")] [JsonRequired] public string Id { get; set; } = null!;
    [JsonPropertyName("version")] [JsonRequired] public WildcardVersion Version { get; set; }
    [JsonPropertyName("type")] public ModDependencyType Type { get; set; } = ModDependencyType.Mandatory;
}