namespace Ignitron.Loader.Metadata;

public interface IModContributor
{
    string Name { get; }
    string? Role { get; }
}