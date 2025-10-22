using System.Text.Json.Serialization;

namespace Ignitron.Loader.Metadata;

/// <summary>
/// Type of mod dependency
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ModDependencyType>))]
public enum ModDependencyType
{
    /// <summary>
    /// This dependency is mandatory, that is, the mod can't work without it
    /// </summary>
    [JsonStringEnumMemberName("mandatory")]
    Mandatory,

    /// <summary>
    /// This dependency is optional, that is, the mod can work without it
    /// </summary>
    [JsonStringEnumMemberName("optional")]
    Optional,

    /// <summary>
    /// This dependency is recommended for improved gameplay, but it isn't mandatory
    /// </summary>
    [JsonStringEnumMemberName("recommended")]
    Recommended,

    /// <summary>
    /// This dependency shouldn't be installed together with the mod and may cause issues
    /// </summary>
    [JsonStringEnumMemberName("not_recommended")]
    NotRecommended,

    /// <summary>
    /// This dependency must not be installed for the mod to work
    /// </summary>
    [JsonStringEnumMemberName("incompatible")]
    Incompatible
}