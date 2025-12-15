using System.Text.Json.Serialization;

namespace Ignitron.Loader.Metadata.Json;

internal sealed class JsonV2ModContributor : IModContributor
{
    [JsonPropertyName("name")] [JsonRequired] public string Name { get; set; }
    [JsonPropertyName("role")] public string? Role { get; }
}