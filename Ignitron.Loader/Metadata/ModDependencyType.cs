using System.Text.Json.Serialization;

namespace Ignitron.Loader.Metadata;

[JsonConverter(typeof(JsonStringEnumConverter<ModDependencyType>))]
public enum ModDependencyType
{
    [JsonStringEnumMemberName("mandatory")]
    Mandatory,

    [JsonStringEnumMemberName("optional")]
    Optional,

    [JsonStringEnumMemberName("recommended")]
    Recommended,

    [JsonStringEnumMemberName("not_recommended")]
    NotRecommended,

    [JsonStringEnumMemberName("incompatible")]
    Incompatible
}