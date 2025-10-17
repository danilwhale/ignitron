using Allumeria.Rendering;
using Ignitron.Aluminium.Assets.Descriptors;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class TextureAssetProvider : IAssetProvider<Texture, TextureAssetDescriptor>
{
    public static TextureAssetProvider Default { get; } = new();
    
    public Texture Create(string assetName, string rootPath, TextureAssetDescriptor descriptor)
    {
        return new Texture(
            Path.Join(rootPath, assetName),
            descriptor.Flip, descriptor.Clamp, descriptor.Mipmaps, descriptor.KeepImage);
    }
}