using System.Buffers;
using Allumeria;
using Allumeria.Rendering;
using Ignitron.Aluminium.Atlases.Sprites;
using OpenTK.Graphics.OpenGL4;
using StbImageWriteSharp;

namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Sprite registry that uses OpenGL texture as storage
/// </summary>
public sealed unsafe class TextureSpriteRegistry : ISpriteRegistry
{
    private readonly record struct SpriteEntry(ISpriteSource Source, in SpriteLocation Location)
    {
        public readonly ISpriteSource Source = Source;
        public readonly SpriteLocation Location = Location;
    }

    /// <summary>
    /// Handle of the OpenGL texture object
    /// </summary>
    public readonly uint Handle;

    /// <summary>
    /// Width of the texture
    /// </summary>
    public readonly uint Width;

    /// <summary>
    /// Height of the texture
    /// </summary>
    public readonly uint Height;

    private readonly ushort _spritesX;
    private readonly ushort _spritesY;
    private readonly Stack<SpriteLocation> _locations = [];
    private readonly Dictionary<SpriteEntry, SpriteLocation> _registeredSprites = [];

    /// <summary>
    /// Creates a new TextureSpriteRegistry over the target texture
    /// </summary>
    /// <param name="textureHandle">Handle of the target texture</param>
    public TextureSpriteRegistry(uint textureHandle)
    {
        Handle = textureHandle;

        GL.BindTexture(TextureTarget.Texture2D, textureHandle);

        GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out int width);
        GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out int height);
        Width = (uint)width;
        Height = (uint)height;
        _spritesX = (ushort)(width / Sprite.SizeInPixels);
        _spritesY = (ushort)(height / Sprite.SizeInPixels);
        ReloadLocations();

        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    /// <summary>
    /// Creates a new TextureSpriteRegistry over the target texture
    /// </summary>
    /// <param name="texture">The target texture</param>
    public TextureSpriteRegistry(Texture texture)
        : this((uint)texture.id)
    {
    }

    private void ReloadLocations()
    {
        _locations.Clear();

        // download atlas pixels from gpu
        byte[] pixels = ArrayPool<byte>.Shared.Rent((int)(Width * Height * 4));
        GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

        // find empty locations
        for (ushort y = 0; y < _spritesY; y++)
        for (ushort x = 0; x < _spritesX; x++)
        {
            if (AtlasHelpers.AreAnyPixelsOccupied(pixels, Width, x, y)) continue;
            _locations.Push(new SpriteLocation(x, y));
        }

        ArrayPool<byte>.Shared.Return(pixels);
        Logger.Init($"{_locations.Count} sprites can be written to atlas with texture handle {Handle}");
    }

    public SpriteLocation Register<TSource>(TSource source, in SpriteLocation location) where TSource : ISpriteSource
    {
        // check if the registry has it already
        if (_registeredSprites.TryGetValue(new SpriteEntry(source, location), out SpriteLocation destination))
        {
            return destination;
        }

        if (!source.TryGetSprite(in location, out ISprite? sprite))
        {
            throw new ArgumentException("Sprite source couldn't provide a sprite at specified location", nameof(location));
        }

        // try to get next free location
        if (!_locations.TryPop(out destination))
        {
            // TODO resize
            throw new InvalidOperationException("Atlas can't fit any more sprites!");
        }

        // insert sprite in a gpu texture
        Span<byte> spritePixels = stackalloc byte[Sprite.SizeInPixels * Sprite.SizeInPixels * 4];
        for (byte y = 0; y < Sprite.SizeInPixels; y++)
        {
            sprite.CopyRowTo(spritePixels.Slice((Sprite.SizeInPixels - 1 - y) * Sprite.SizeInPixels * 4, Sprite.SizeInPixels * 4), y);
        }

        GL.BindTexture(TextureTarget.Texture2D, Handle);

        fixed (byte* pSpritePixels = spritePixels)
        {
            GL.TexSubImage2D(
                TextureTarget.Texture2D,
                0,
                destination.X * Sprite.SizeInPixels,
                destination.Y * Sprite.SizeInPixels,
                Sprite.SizeInPixels,
                Sprite.SizeInPixels,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                (nint)pSpritePixels);
        }

        GL.BindTexture(TextureTarget.Texture2D, 0);

        // store in the registry
        _registeredSprites[new SpriteEntry(source, location)] = destination;

        return destination;
    }

    public bool TryRegister<TSource>(TSource source, in SpriteLocation location, out SpriteLocation destination) where TSource : ISpriteSource
    {
        // check if the registry has it already
        if (_registeredSprites.TryGetValue(new SpriteEntry(source, location), out destination))
        {
            return true;
        }

        if (!source.TryGetSprite(in location, out ISprite? sprite))
        {
            destination = default;
            return false;
        }

        // try to get next free location
        if (!_locations.TryPop(out destination))
        {
            // TODO resize
            destination = default;
            return false;
        }

        // insert sprite in a gpu texture
        Span<byte> spritePixels = stackalloc byte[Sprite.SizeInPixels * Sprite.SizeInPixels];
        for (byte y = 0; y < Sprite.SizeInPixels; y++)
        {
            sprite.CopyRowTo(spritePixels.Slice(y * Sprite.SizeInPixels, Sprite.SizeInPixels), y);
        }

        GL.BindTexture(TextureTarget.Texture2D, Handle);

        fixed (byte* pSpritePixels = spritePixels)
        {
            GL.TexSubImage2D(
                TextureTarget.Texture2D,
                0,
                destination.X * Sprite.SizeInPixels,
                destination.Y * Sprite.SizeInPixels,
                Sprite.SizeInPixels,
                Sprite.SizeInPixels,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                (nint)pSpritePixels);
        }

        GL.BindTexture(TextureTarget.Texture2D, 0);

        // store in the registry
        _registeredSprites[new SpriteEntry(source, location)] = destination;

        return true;
    }
    
    /// <summary>
    /// Dumps backing OpenGL texture data to the target stream using given <see cref="ImageWriter"/>
    /// </summary>
    /// <param name="writer">Writer which will be used to write the texture data</param>
    /// <param name="destination">Destination stream to which texture data will be written</param>
    public void DumpPng(ImageWriter writer, Stream destination)
    {
        GL.BindTexture(TextureTarget.Texture2D, Handle);

        // download atlas pixels from gpu
        byte[] pixels = ArrayPool<byte>.Shared.Rent((int)(Width * Height * 4));
        GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

        StbImageWrite.stbi_flip_vertically_on_write(1);
        writer.WritePng(pixels, (int)Width, (int)Height, ColorComponents.RedGreenBlueAlpha, destination);
        StbImageWrite.stbi_flip_vertically_on_write(0);

        ArrayPool<byte>.Shared.Return(pixels);
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    /// <summary>
    /// Dumps backing OpenGL texture data to the target file using given <see cref="ImageWriter"/>
    /// </summary>
    /// <param name="writer">Writer which will be used to write the texture data</param>
    /// <param name="filePath">Destination file path to which texture data will be written</param>
    public void DumpPng(ImageWriter writer, string filePath)
    {
        using FileStream fs = File.OpenWrite(filePath);
        DumpPng(writer, fs);
    }
}