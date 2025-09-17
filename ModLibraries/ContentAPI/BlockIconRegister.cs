using System.Buffers;
using System.Diagnostics;
using HarmonyLib;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PocketBlocks.Rendering;
using StbImageSharp;
using StbImageWriteSharp;
using ColorComponents = StbImageWriteSharp.ColorComponents;

namespace ContentAPI;

public sealed class BlockIconRegister : IIconRegister
{
    public static readonly BlockIconRegister Instance = new();

    private static Texture _atlasTexture = null!;
    private static readonly Stack<Vector2i> FreeCells = [];

    internal static void Init()
    {
        _atlasTexture = (Texture)AccessTools.Field("PocketBlocks.Rendering.Drawing:defaultTexture").GetValue(null)!;
        _atlasTexture.Use();

        // find texture size
        GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out int width);
        GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out int height);

        // download texture data
        byte[] atlasPixels = ArrayPool<byte>.Shared.Rent(width * height * 4);
        GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, atlasPixels);

        // find empty cells
        for (int cy = 0; cy < height; cy += 16)
        for (int cx = 0; cx < width; cx += 16)
        {
            CheckAtlasCell(atlasPixels, width, cx, cy);
        }

        ArrayPool<byte>.Shared.Return(atlasPixels);

        Console.WriteLine("ContentAPI: {0} cells are free in 'terrain.png'", FreeCells.Count);
    }

    private static void CheckAtlasCell(byte[] atlasPixels, int atlasWidth, int cx, int cy)
    {
        // verify that cell is completely transparent
        for (int y = 0; y < 16; y++)
        for (int x = 0; x < 16; x++)
        {
            if (atlasPixels[((cy + y) * atlasWidth + (cx + x)) * 4 + 3] > 0) return;
        }

        // add free cell
        FreeCells.Push(new Vector2i(cx, cy));
    }

    private readonly Dictionary<string, Icon> _icons = [];

    public Icon? Register(string key, ImageResult image)
    {
        if (_icons.TryGetValue(key, out Icon icon)) return icon;

        if (!FreeCells.TryPop(out Vector2i newCell))
        {
            Debug.Assert(false, "all cells have been taken by other mods!");
            return null;
        }
        
        _atlasTexture.Use();
        GL.TexSubImage2D(TextureTarget.Texture2D, 0, newCell.X, newCell.Y, image.Width, image.Height, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        Console.WriteLine("registered '{0}' to {1}", key, newCell);
        return _icons[key] = new Icon(newCell.X, newCell.Y, image.Width, image.Height);
    }

    public Icon? this[string key] => !_icons.TryGetValue(key, out Icon icon) ? icon : null;
}