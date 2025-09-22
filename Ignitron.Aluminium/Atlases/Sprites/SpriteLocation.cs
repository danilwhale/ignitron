using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ignitron.Aluminium.Atlases.Sprites;

/// <summary>
/// Description of a sprite location in the atlas
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 4)]
public readonly struct SpriteLocation(ushort x, ushort y) : IEquatable<SpriteLocation>, ISpanFormattable
{
    public const int SizeInBytes = 2 * sizeof(ushort);

    /// <summary>
    /// Location of this sprite inside the atlas on X axis in <see cref="Sprite.SizeInPixels"/> units 
    /// </summary>
    public readonly ushort X = x;

    /// <summary>
    /// Location of this sprite inside the atlas on Y axis in <see cref="Sprite.SizeInPixels"/> units 
    /// </summary>
    public readonly ushort Y = y;
    
    /// <summary>
    /// Location of this sprite inside the atlas image on X axis in pixel units
    /// </summary>
    public uint AtlasX
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (uint)X * Sprite.SizeInPixels;
    }

    /// <summary>
    /// Location of this sprite inside the atlas image on Y axis in pixel units
    /// </summary>
    public uint AtlasY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (uint)Y * Sprite.SizeInPixels;
    }

    public bool Equals(SpriteLocation other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is SpriteLocation other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(SpriteLocation left, SpriteLocation right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SpriteLocation left, SpriteLocation right)
    {
        return !left.Equals(right);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        FormattableString formattable = $"({X}{NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator}{Y})";
        return formattable.ToString(formatProvider);
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return destination.TryWrite(provider, $"({X}{NumberFormatInfo.GetInstance(provider).NumberGroupSeparator}{Y})", out charsWritten);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}