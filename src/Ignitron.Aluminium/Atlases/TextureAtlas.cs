using System.Buffers;
using System.Runtime.CompilerServices;
using Allumeria.Rendering;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using StbImageWriteSharp;

namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// A wrapper around the texture object that allows to stitch new sprites on it
/// </summary>
public sealed unsafe class TextureAtlas : IStitcher, IAtlas
{
    private readonly record struct SpriteView(ISprite Value)
    {
        public readonly int Width = Value.Width;
        public readonly int Height = Value.Height;
    }

    private sealed class Slot(int x, int y, int width, int height)
    {
        public readonly int X = x;
        public readonly int Y = y;
        public readonly int Width = width;
        public readonly int Height = height;

        public SpriteView? Sprite;
        public readonly Lazy<List<Slot>> Children = new(() => []);

        public bool TryAdd(ref readonly SpriteView view, out StitchedSprite stitched)
        {
            if (Sprite != null)
            {
                stitched = default;
                return false;
            }

            // check if sprite can fit into this slot
            // if it can't fit here it totally won't fit in any of the children slots
            if (view.Width > Width || view.Height > Height)
            {
                stitched = default;
                return false;
            }

            // if sprite is exact size of our slot then we can just set '_sprite' and return
            if (view.Width == Width && view.Height == Height)
            {
                Sprite = view;
                stitched = new StitchedSprite((ushort)X, (ushort)Y, (ushort)(X + Width), (ushort)(Y + Height));
                return true;
            }

            List<Slot> children;
            if (!Children.IsValueCreated)
            {
                // now we create children slots so we can assign a sprite to one of them
                children = Children.Value;
                children.Add(new Slot(X, Y, view.Width, view.Height));

                int freeWidth = Width - view.Width;
                int freeHeight = Height - view.Height;

                if (freeWidth > 0 && freeHeight > 0)
                {
                    // create a strip and a big blob
                    if (Math.Max(Width, freeHeight) >= Math.Max(Height, freeWidth))
                    {
                        // generate a strip on horizontal axis and a blob of remaining space
                        children.Add(new Slot(X + view.Width, Y, freeWidth, view.Height));
                        children.Add(new Slot(X, Y + view.Height, Width, freeHeight));
                    }
                    else
                    {
                        // generate a strip on vertical axis and a blob of remaining space
                        children.Add(new Slot(X, Y + view.Height, view.Width, freeHeight));
                        children.Add(new Slot(X + view.Width, Y, freeWidth, Height));
                    }
                }
                else if (freeWidth > 0)
                {
                    // generate a strip on horizontal axis
                    children.Add(new Slot(X + view.Width, Y, freeWidth, view.Height));
                }
                else if (freeHeight > 0)
                {
                    // generate a strip on vertical axis
                    children.Add(new Slot(X, Y + view.Height, view.Width, freeHeight));
                }
            }
            else children = Children.Value;

            // try to assign a sprite to one of the children slots
            foreach (Slot child in children)
            {
                if (child.TryAdd(in view, out stitched))
                {
                    return true;
                }
            }

            stitched = default;
            return false;
        }
    }

    /// <summary>
    /// The texture object that is getting wrapped
    /// </summary>
    public Texture Texture { get; }
    
    /// <summary>
    /// A value indicating whether the texture is flipped vertically
    /// </summary>
    public bool IsFlipped { get; }

    public int Width => _width;
    public int Height => _height;
    
    /// <summary>
    /// Base size of every slot during the slots generation
    /// </summary>
    public int BaseSlotSize { get; }

    private readonly int _width;
    private readonly int _height;

    private readonly List<Slot> _slots = [];
    private readonly Dictionary<string, StitchedSprite> _sprites = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="TextureAtlas"/> that will wrap the given texture object
    /// </summary>
    /// <param name="texture">The texture object to wrap</param>
    /// <param name="flipped">A value indicating whether the texture is flipped vertically</param>
    /// <param name="baseSlotSize">Base size of every slot during the slots generation</param>
    public TextureAtlas(Texture texture, bool flipped, int baseSlotSize)
    {
        Texture = texture;
        IsFlipped = flipped;
        BaseSlotSize = baseSlotSize;

        if (texture.sourceImage != null)
        {
            // use cpu image
            _width = texture.sourceImage.Width;
            _height = texture.sourceImage.Height;
            FindSlots(texture.sourceImage.Data);
        }
        else
        {
            // use gpu image
            GL.BindTexture(TextureTarget.Texture2D, texture.id);

            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out _width);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out _height);

            byte[] pixels = ArrayPool<byte>.Shared.Rent(Width * Height * 4);
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            if (flipped)
            {
                fixed (byte* pPixels = pixels)
                {
                    StbImage.stbi__vertical_flip(pPixels, Width, Height, 4);
                }
            }

            FindSlots(pixels);
            ArrayPool<byte>.Shared.Return(pixels);
        }
    }

    // human obfuscation has been performed
    private void FindSlots(byte[] pixels)
    {
        int slotsX = Width / BaseSlotSize;
        int slotsY = Height / BaseSlotSize;

        // we store 32 slots per 1 value. that is, we use 1 bit to determine whether slot is valid or not
        Span<uint> validSlots = stackalloc uint[((slotsX * slotsY) - 1 + (1 << 5)) >> 5];

        // compute slots (NxN areas) that are opaque, i.e. have pixels in them
        int n = 0; // index of the slot
        for (int y = 0; y < slotsY; y++)
        for (int x = 0; x < slotsX; x++)
        {
            uint bm = (uint)(1 << n); // bit mask (which bit we're going to update)

            // iterate over NxN area to find any non-transparent pixel (anything with alpha > 0)
            bool o = true; // whether the slot is opaque
            for (int yy = 0; yy < BaseSlotSize; yy++)
            for (int xx = 0; xx < BaseSlotSize; xx++)
            {
                // check if alpha channel is blank
                if (pixels[(((y * BaseSlotSize) + yy) * Width + ((x * BaseSlotSize) + xx)) * 4 + 3] != 0)
                {
                    o = false;
                    goto writeBit;
                }
            }

            writeBit:
            // write a bit of the slot 
            ref uint s = ref validSlots[n >> 5]; // get reference to the part of the bit array we're in right now
            if (o) s |= bm; // slot is opaque
            else s &= ~bm; // slot is transparent

            n++;
        }

        // this is similar to greedy meshing, we connect non-opaque areas into 1 big slot
        // then we also invalidate the iterated slots
        n = 0;
        for (int y = 0; y < slotsY; y++)
        for (int x = 0; x < slotsX;)
        {
            // get bit of the slot
            if (IsValidSlot(validSlots, n))
            {
                // TODO maybe change iteration direction to get less single columns?
                int w, h;
                // increment width until we hit an invalid slot
                for (w = 1; x + w < slotsX && IsValidSlot(validSlots, n + w); w++) ;

                // height is a bit interesting, we also check every line for valid slots before incrementing
                bool done = false;
                for (h = 1; y + h < slotsY; h++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        int si = n + h * slotsX + i;

                        // we've hit an invalid slot, this is the end of the slot
                        if (!IsValidSlot(validSlots, si))
                        {
                            done = true;
                            break;
                        }

                        // also invalidate slots as we go, to save some cpu cycles
                        validSlots[si >> 5] &= ~(uint)(1 << si);
                    }

                    if (done) break;
                }

                // add slot to the storage
                _slots.Add(new Slot(x * BaseSlotSize, y * BaseSlotSize, w * BaseSlotSize, h * BaseSlotSize));
                n += w;
                x += w;
            }
            else
            {
                n++;
                x++;
            }
        }

        return;

        // a little macro so i don't have to write this abomination over and over
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsValidSlot(Span<uint> validSlots, int index) => (validSlots[index >> 5] & (1 << index)) != 0;
    }

    /// <summary>
    /// Debug method to dump the stored slots in the given stream as a PNG image
    /// </summary>
    /// <param name="stream">The stream to write a PNG image to</param>
    public void DumpDebugSlots(Stream stream)
    {
        byte[] pixels = ArrayPool<byte>.Shared.Rent(Width * Height * 4);
        if (Texture.sourceImage != null)
        {
            Texture.sourceImage.Data.CopyTo(pixels.AsSpan());
        }
        else
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.id);
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
            if (IsFlipped)
            {
                fixed (byte* pPixels = pixels)
                {
                    StbImage.stbi__vertical_flip(pPixels, Width, Height, 4);
                }
            }
        }

        foreach (Slot slot in _slots)
        {
            BlitSlot(slot);
        }

        ImageWriter iw = new();
        iw.WritePng(pixels, Width, Height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, stream);

        ArrayPool<byte>.Shared.Return(pixels);

        void BlitSlot(Slot slot)
        {
            if (slot.Children.IsValueCreated)
            {
                foreach (Slot child in slot.Children.Value)
                {
                    BlitSlot(child);
                }

                return;
            }

            if (slot.Sprite != null) return;

            int c = HashCode.Combine(slot.X, slot.Y, slot.Width, slot.Height);
            byte r = (byte)((c >> 16) & 0xFF);
            byte g = (byte)((c >> 8) & 0xFF);
            byte b = (byte)(c & 0xFF);
            for (int x = 0; x < slot.Width; x++)
            for (int y = 0; y < slot.Height; y++)
            {
                int n = ((slot.Y + y) * Width + (slot.X + x)) * 4;
                pixels[n] = r;
                pixels[n + 1] = g;
                pixels[n + 2] = b;
                pixels[n + 3] = 255;
            }
        }
    }

    public StitchedSprite GetSprite(string name)
    {
        return _sprites[name];
    }

    public bool TryGetSprite(string name, out StitchedSprite stitchedSprite)
    {
        return _sprites.TryGetValue(name, out stitchedSprite);
    }

    public StitchedSprite AddSprite(string name, ISprite sprite)
    {
        if (!TryAddSprite(name, sprite, out StitchedSprite stitchedSprite))
        {
            throw new InvalidOperationException("Couldn't fit place for a sprite in the atlas!");
        }

        return stitchedSprite;
    }

    public bool TryAddSprite(string name, ISprite sprite, out StitchedSprite stitchedSprite)
    {
        // IMPORTANT we have to stitch sprites on-the-fly, as we can't change UV coordinates of
        //           the block or item we haven't been provided through Stitch method
        if (_sprites.TryGetValue(name, out stitchedSprite))
        {
            return true;
        }

        SpriteView view = new(sprite);
        foreach (Slot slot in _slots)
        {
            if (slot.TryAdd(in view, out stitchedSprite))
            {
                ushort w = stitchedSprite.Width, h = stitchedSprite.Height;

                // copy sprite into a buffer
                Span<byte> buffer = stackalloc byte[w * h * 4];
                for (int i = 0; i < h; i++)
                {
                    sprite.CopyRowTo(IsFlipped ? h - i - 1 : i, buffer.Slice(i * w * 4, w * 4));
                }

                // send sprite to gpu
                GL.BindTexture(TextureTarget.Texture2D, Texture.id);
                GL.TexSubImage2D(
                    TextureTarget.Texture2D,
                    0,
                    stitchedSprite.U0, IsFlipped ? Height - stitchedSprite.V1 : stitchedSprite.V0, w, h,
                    PixelFormat.Rgba, PixelType.UnsignedByte,
                    (nint)Unsafe.AsPointer(ref buffer.GetPinnableReference()) // should be safe to do as span never escapes the stack
                );

                _sprites[name] = stitchedSprite;
                return true;
            }
        }

        // TODO: expand
        stitchedSprite = default;
        return false;
    }
}