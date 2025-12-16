namespace Ignitron.Loader.Metadata;

/// <summary>
/// Represents a mod contributor
/// </summary>
/// <param name="name">Name of a mod contributor</param>
/// <param name="role">Role of a mod contributor</param>
public readonly struct ModContributor(string name, string? role) : IModContributor
{
    /// <inheritdoc />
    public string Name { get; } = name;

    /// <inheritdoc />
    public string? Role { get; } = role;
}