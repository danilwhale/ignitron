using System.Text.Json.Serialization;

namespace Ignitron.Loader.Metadata;

internal sealed class LegacyJsonModDependency : IModDependency
{
    public bool Optional { get; set; }
    
    [JsonRequired] public string Id { get; set; }
    [JsonRequired] public WildcardVersion Version { get; set; }
    public ModDependencyType Type => Optional ? ModDependencyType.Optional : ModDependencyType.Mandatory;
}