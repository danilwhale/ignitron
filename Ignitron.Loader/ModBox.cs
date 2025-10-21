using Ignitron.Loader.Metadata;

namespace Ignitron.Loader;

public sealed class ModBox(IModMetadata metadata, string rootPath, string assemblyPath)
{
    public IModMetadata Metadata { get; } = metadata;
    public string RootPath { get; } = rootPath;
    public string AssemblyPath { get; } = assemblyPath;

    public ModBox(IModMetadata metadata, string rootPath)
        : this(metadata, rootPath, Path.Join(rootPath, metadata.AssemblyRelativePath))
    {
    }
}