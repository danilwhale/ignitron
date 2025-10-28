using Ignitron.Aluminium.Assets.Descriptors;
using Ignitron.Aluminium.Atlases;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class SpriteAssetProvider : IAssetProvider<ImageSprite>
{
    public static readonly SpriteAssetProvider Default = new();

    public ImageSprite Create(AssetManager assets, string assetName)
    {
        using Stream stream = assets.Open(assetName);
        using MemoryStream ms = new();
        stream.CopyTo(ms);
        ms.Position = 0;

        return ImageSprite.FromStream(stream);
    }
}