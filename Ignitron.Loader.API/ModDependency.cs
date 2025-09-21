using System.Text.Json.Serialization;
using Ignitron.Loader.API.Versioning;

namespace Ignitron.Loader.API;

public sealed class ModDependency
{
    [JsonRequired] public string Id { get; set; } // mod id of the dependency
    [JsonRequired] public WildcardVersion Version { get; set; } // version of the dependency this mod was made for
    public bool Optional { get; set; } // whether this dependency is optional, by default all dependencies are mandatory
}