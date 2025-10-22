using Ignitron.Loader.Metadata;

namespace Ignitron.Loader;

/// <summary>
/// Represents a collection of details about a mod
/// </summary>
/// <param name="metadata">Metadata details about a mod</param>
/// <param name="rootPath">Absolute path to the directory of a mod</param>
/// <param name="assemblyPath">Absolute path to the assembly file of a mod</param>
public sealed class ModBox(IModMetadata metadata, string rootPath, string assemblyPath)
{
    /// <summary>
    /// Metadata details about a mod
    /// </summary>
    public IModMetadata Metadata { get; } = metadata;
    
    /// <summary>
    /// Absolute path to the directory of a mod
    /// </summary>
    public string RootPath { get; } = rootPath;
    
    /// <summary>
    /// Absolute path to the assembly file of a mod
    /// </summary>
    public string AssemblyPath { get; } = assemblyPath;

    /// <summary>
    /// Initialises a new instance of the <see cref="ModBox"/> class with assembly path pointing to <see cref="IModMetadata.AssemblyRelativePath"/> relative to <paramref name="rootPath"/>
    /// </summary>
    /// <param name="metadata">Metadata details about a mod</param>
    /// <param name="rootPath">Absolute path to the directory of a mod</param>
    public ModBox(IModMetadata metadata, string rootPath)
        : this(metadata, rootPath, Path.Join(rootPath, metadata.AssemblyRelativePath))
    {
    }
}