namespace Ignitron.Loader.Metadata;

/// <summary>
/// Represents a mod dependency
/// </summary>
public interface IModDependency
{
    /// <summary>
    /// Unique mod ID of a dependency
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Version of a dependency specified using wildcard
    /// </summary>
    /// <example><c>0.*.0</c></example>
    WildcardVersion Version { get; }

    /// <summary>
    /// Type of dependency
    /// </summary>
    ModDependencyType Type { get; }
}