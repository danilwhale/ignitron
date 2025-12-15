using StbImageSharp;

namespace Ignitron.Aluminium.Assets.Descriptors;

public readonly record struct ImageAssetDescriptor(ColorComponents RequiredComponents = ColorComponents.Default)
    : IAssetDescriptor<ImageResult>;