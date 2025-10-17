using Allumeria.DataManagement.ModelParsing;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class ModelAssetProvider : IAssetProvider<BBModel>
{
    public static ModelAssetProvider Default { get; } = new();
    
    public BBModel Create(string assetName, string rootPath)
    {
        return ModelParser.ParseModel(Path.Join(rootPath, assetName));
    }
}