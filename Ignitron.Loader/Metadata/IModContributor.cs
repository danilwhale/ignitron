namespace Ignitron.Loader.Metadata;

/// <summary>
/// Represents a mod contributor
/// </summary>
public interface IModContributor
{
    /// <summary>
    /// Name of a mod contributor
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Role of a mod contributor
    /// </summary>
    string? Role { get; }
}