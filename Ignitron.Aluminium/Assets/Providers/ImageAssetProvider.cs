using Ignitron.Aluminium.Assets.Descriptors;
using StbImageSharp;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class ImageAssetProvider : IAssetProvider<ImageResult, ImageAssetDescriptor>
{
    public static ImageAssetProvider Default { get; } = new();
    
    public ImageResult Create(string assetName, string rootPath, ImageAssetDescriptor descriptor)
    {
        using FileStream stream = File.OpenRead(Path.Join(rootPath, assetName));
        return ImageResult.FromStream(stream, descriptor.RequiredComponents);
    }
}