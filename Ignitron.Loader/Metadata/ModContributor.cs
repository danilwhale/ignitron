namespace Ignitron.Loader.Metadata;

public readonly struct ModContributor(string name, string? role) : IModContributor
{
    public string Name { get; } = name;
    public string? Role { get; } = role;
}