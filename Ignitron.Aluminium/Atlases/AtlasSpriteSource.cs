using System.Diagnostics.CodeAnalysis;
using Ignitron.Aluminium.Atlases.Sprites;
using StbImageSharp;

namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Sprite source that returns sprite at given location from the atlas
/// </summary>
public sealed class AtlasSpriteSource : ISpriteSource
{
    /// <summary>
    /// Width of the atlas in pixels
    /// </summary>
    public readonly uint Width;

    /// <summary>
    /// Height of the atlas in pixels
    /// </summary>
    public readonly uint Height;

    private readonly ushort _spritesX;
    private readonly ushort _spritesY;
    private readonly Dictionary<SpriteLocation, Sprite> _sprites;

    /// <summary>
    /// Creates a new AtlasSpriteSource from the target image
    /// </summary>
    /// <param name="pixels">Pixel data of the image in RGBA order</param>
    /// <param name="width">Width of the image</param>
    /// <param name="height">Height of the image</param>
    public AtlasSpriteSource(ReadOnlySpan<byte> pixels, uint width, uint height)
    {
        Width = width;
        Height = height;

        _spritesX = (ushort)(width / Sprite.SizeInPixels);
        _spritesY = (ushort)(height / Sprite.SizeInPixels);

        _sprites = new Dictionary<SpriteLocation, Sprite>((int)((uint)_spritesX * _spritesY));
        for (ushort y = 0; y < _spritesY; y++)
        for (ushort x = 0; x < _spritesX; x++)
        {
            if (!AtlasHelpers.AreAnyPixelsOccupied(pixels, width, x, y)) continue;

            byte[] spritePixels = new byte[Sprite.SizeInPixels * Sprite.SizeInPixels];
            AtlasHelpers.CopySpritePixels(pixels, width, x, y, spritePixels);
            _sprites[new SpriteLocation(x, y)] = new Sprite(spritePixels);
        }
    }

    public bool TryGetSprite(in SpriteLocation location, [NotNullWhen(true)] out ISprite? sprite)
    {
        if (location.X >= _spritesX || location.Y >= _spritesY || !_sprites.TryGetValue(location, out Sprite? s))
        {
            sprite = null;
            return false;
        }

        sprite = s;
        return true;
    }

    /// <summary>
    /// Creates a new AtlasSpriteSource from the target image stream
    /// </summary>
    public static AtlasSpriteSource FromStream(Stream stream)
    {
        StbImage.stbi_set_flip_vertically_on_load(0);
        ImageResult image = ImageResult.FromStream(stream);
        return new AtlasSpriteSource(image.Data, (uint)image.Width, (uint)image.Height);
    }

    /// <summary>
    /// Creates a new AtlasSpriteSource from the target image file
    /// </summary>
    public static AtlasSpriteSource FromFile(string filePath)
    {
        using Stream stream = File.OpenRead(filePath);
        return FromStream(stream);
    }
}