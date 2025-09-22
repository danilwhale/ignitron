using Ignitron.Aluminium.Atlases.Sprites;

namespace Ignitron.Aluminium.Atlases;

internal static class AtlasHelpers
{
    public static bool AreAnyPixelsOccupied(ReadOnlySpan<byte> atlasPixels, uint atlasWidth, ushort spriteX, ushort spriteY)
    {
        uint atlasX = spriteX * (uint)Sprite.SizeInPixels;
        uint atlasY = spriteY * (uint)Sprite.SizeInPixels;

        for (uint y = atlasY; y < atlasY + Sprite.SizeInPixels; y++)
        for (uint x = atlasX; x < atlasX + Sprite.SizeInPixels; x++)
        {
            // check if alpha channel is not empty
            if (atlasPixels[(int)((y * atlasWidth + x) * 4 + 3)] != 0)
            {
                return true;
            }
        }

        return false;
    }

    public static void CopySpritePixels(ReadOnlySpan<byte> atlasPixels, uint atlasWidth, uint spriteX, uint spriteY, Span<byte> destination)
    {
        uint atlasX = spriteX * Sprite.SizeInPixels;
        uint atlasY = spriteY * Sprite.SizeInPixels;

        for (uint y = 0; y < Sprite.SizeInPixels; y++)
        {
            ReadOnlySpan<byte> sourceRow = atlasPixels.Slice((int)((y + atlasY) * atlasWidth + atlasX), Sprite.SizeInPixels);
            Span<byte> destinationRow = destination.Slice((int)y, Sprite.SizeInPixels);
            sourceRow.CopyTo(destination);
        }
    }
}