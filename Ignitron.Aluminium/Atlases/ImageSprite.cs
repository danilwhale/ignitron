using StbImageSharp;

namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Wraps the image in a sprite representation
/// </summary>
public sealed class ImageSprite : ISprite
{
    public int Width { get; }
    public int Height { get; }
    
    /// <summary>
    /// Pixel stride of the sprite
    /// </summary>
    public int Stride { get; }
    
    /// <summary>
    /// Pixel data of the sprite
    /// </summary>
    public byte[] Pixels { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageSprite"/> class that will wrap the given image data
    /// </summary>
    /// <param name="width">Width of the image</param>
    /// <param name="height">Height of the image</param>
    /// <param name="pixels">Pixel data (RGBA8) of the image</param>
    /// <exception cref="ArgumentException">pixels are provided not in RGBA8 format</exception>
    public ImageSprite(int width, int height, byte[] pixels)
    {
        if (pixels.Length != width * height * 4)
        {
            throw new ArgumentException("Pixels must be stored in RGBA format", nameof(pixels));
        }
        
        Width = width;
        Height = height;
        Stride = width * 4;
        Pixels = pixels;
    }

    public void CopyRowTo(int rowIndex, Span<byte> destination)
    {
        Pixels.AsSpan(Stride * rowIndex, Stride).CopyTo(destination);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageSprite"/> class that will wrap an image inside the given stream
    /// </summary>
    /// <param name="stream">The stream to read image from</param>
    public static ImageSprite FromStream(Stream stream)
    {
        ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        return new ImageSprite(image.Width, image.Height, image.Data);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageSprite"/> class that will wrap an image inside the given file
    /// </summary>
    /// <param name="path">A path of the file to read image from</param>
    public static ImageSprite FromFile(string path)
    {
        using FileStream stream = File.OpenRead(path);
        return FromStream(stream);
    }
}