namespace Ignitron.Loader.Metadata;

public readonly struct ModContributor(string name, string? role)
{
    public string Name { get; } = name;
    public string? Role { get; } = role;
}