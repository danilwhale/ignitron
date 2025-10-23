using System.Text.Json;
using Allumeria.DataManagement.ModelParsing;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class ModelAssetProvider : IAssetProvider<BBModel>
{
    public static ModelAssetProvider Default { get; } = new();

    public BBModel Create(AssetManager assets, string assetName)
    {
        using Stream stream = assets.Open(assetName);
        JsonDocument jsonDocument = JsonDocument.Parse(stream);
        BBModel model = new(jsonDocument.RootElement);
        model.mesh = model.ConvertToMesh();
        return model;
    }
}