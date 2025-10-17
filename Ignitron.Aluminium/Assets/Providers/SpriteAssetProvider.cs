using Ignitron.Aluminium.Assets.Descriptors;
using Ignitron.Aluminium.Atlases;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class SpriteAssetProvider : IAssetProvider<ImageSprite>
{
    public static readonly SpriteAssetProvider Default = new();
    
    public ImageSprite Create(string assetName, string rootPath)
    {
        using FileStream stream = File.OpenRead(Path.Join(rootPath, assetName));
        return ImageSprite.FromStream(stream);
    }
}