using Ignitron.Aluminium.Assets.Descriptors;
using StbImageSharp;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class ImageAssetProvider : IAssetProvider<ImageResult, ImageAssetDescriptor>
{
    public static ImageAssetProvider Default { get; } = new();

    public ImageResult Create(AssetManager assets, string assetName, ImageAssetDescriptor descriptor)
    {
        using Stream stream = assets.Open(assetName);
        using MemoryStream ms = new();
        stream.CopyTo(ms);
        ms.Position = 0;

        return ImageResult.FromStream(stream, descriptor.RequiredComponents);
    }
}