using StbImageSharp;

namespace Ignitron.Aluminium.Atlases;

public sealed class ImageSprite : ISprite
{
    public int Width { get; }
    public int Height { get; }
    public int Stride { get; }
    public byte[] Pixels { get; }
    
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

    public static ImageSprite FromStream(Stream stream)
    {
        ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        return new ImageSprite(image.Width, image.Height, image.Data);
    }

    public static ImageSprite FromFile(string path)
    {
        using FileStream stream = File.OpenRead(path);
        return FromStream(stream);
    }
}