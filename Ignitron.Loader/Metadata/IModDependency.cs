namespace Ignitron.Loader.Metadata;

public interface IModDependency
{
    string Id { get; }
    WildcardVersion Version { get; }
    ModDependencyType Type { get; }
}