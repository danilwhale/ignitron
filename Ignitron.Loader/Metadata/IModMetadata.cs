namespace Ignitron.Loader.Metadata;

/// <summary>
/// Represents details about a mod specified through metadata file or other source
/// </summary>
public interface IModMetadata
{
    /// <summary>
    /// Relative path to the assembly file of a mod
    /// </summary>
    /// <example><c>"ExampleMod.dll"</c></example>
    string AssemblyRelativePath { get; }

    /// <summary>
    /// Unique ID of a mod that can be used globally (for example, in <see cref="IModDependency"/>)
    /// </summary>
    /// <example><c>example_mod</c></example>
    string Id { get; }

    /// <summary>
    /// Display name of a mod
    /// </summary>
    /// <example><c>"Example mod"</c></example>
    string DisplayName { get; }

    /// <summary>
    /// Contributors in the development process of a mod
    /// </summary>
    IEnumerable<IModContributor> Contributors { get; }

    /// <summary>
    /// Optional description of a mod
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Version of a mod. 
    /// </summary>
    /// <example><c>0.1.0</c></example>
    Version Version { get; }

    /// <summary>
    /// Other mods that are referenced, incompatible or suggested to use by this mod
    /// </summary>
    IEnumerable<IModDependency> Dependencies { get; }

    /// <summary>
    /// Names of types within the mod assembly that implement <see cref="IModEntrypoint"/>
    /// </summary>
    IEnumerable<string> Entrypoints { get; }
}