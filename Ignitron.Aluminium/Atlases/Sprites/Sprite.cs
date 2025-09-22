using StbImageSharp;

namespace Ignitron.Aluminium.Atlases.Sprites;

/// <summary>
/// Instance of a sprite, storing its pixel data
/// </summary>
public sealed class Sprite : ISprite
{
    /// <summary>
    /// Size of each sprite in an atlas in pixels
    /// </summary>
    public const int SizeInPixels = 16;

    /// <summary>
    /// Pixel data of the sprite in RGBA order
    /// </summary>
    public readonly byte[] Pixels;

    public Sprite(byte[] pixels)
    {
        if (pixels.Length != SizeInPixels * SizeInPixels * 4)
        {
            throw new ArgumentException("Pixel array must have length of 16x16x4");
        }

        Pixels = pixels;
    }

    public void CopyRowTo(scoped Span<byte> destination, byte row)
    {
        Pixels.AsSpan(row * SizeInPixels * 4, SizeInPixels * 4).CopyTo(destination);
    }

    /// <summary>
    /// Reads a sprite from given stream
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when sprite resolution doesn't equal to 16x16</exception>
    public static Sprite FromStream(Stream stream)
    {
        ImageResult image = ImageResult.FromStream(stream);

        if (image.Width != SizeInPixels || image.Height != SizeInPixels)
        {
            // TODO handle sprites of various resolutions
            throw new InvalidOperationException("Sprites with resolution other than 16x are not supported yet, sorry!");
        }

        return new Sprite(image.Data);
    }

    /// <summary>
    /// Reads a sprite from given file path
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when sprite resolution doesn't equal to 16x16</exception>
    public static Sprite FromFile(string filePath)
    {
        using Stream stream = File.OpenRead(filePath);
        return FromStream(stream);
    }
}