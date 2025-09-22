namespace Ignitron.Aluminium.Atlases.Sprites;

/// <summary>
/// Instance of a sprite that doesn't contain any data
/// </summary>
public sealed class BlankSprite : ISprite
{
    public void CopyRowTo(scoped Span<byte> destination, byte row) => destination.Clear();
}