using Allumeria.Blocks.Structures;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class StructureAssetProvider : IAssetProvider<Structure>
{
    public static StructureAssetProvider Default { get; } = new();
    
    public Structure Create(string assetName, string rootPath)
    {
        Structure structure = new();
        structure.ReadFromFilePath(Path.Join(rootPath, assetName));
        return structure;
    }
}