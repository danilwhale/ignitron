using Allumeria.Rendering;

namespace Ignitron.Aluminium.Assets.Descriptors;

public readonly record struct TextureAssetDescriptor(bool Flip, bool Clamp, bool Mipmaps, bool KeepImage = false)
    : IAssetDescriptor<Texture>;