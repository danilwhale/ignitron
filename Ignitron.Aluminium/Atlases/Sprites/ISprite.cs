namespace Ignitron.Aluminium.Atlases.Sprites;

public interface ISprite
{
    /// <summary>
    /// Copies row of a pixel data from this sprite to a specified buffer
    /// </summary>
    void CopyRowTo(scoped Span<byte> destination, byte row);
}