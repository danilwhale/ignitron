namespace Ignitron.Aluminium.Atlases;

public interface ISprite
{
    int Width { get; }
    int Height { get; }

    void CopyRowTo(int rowIndex, Span<byte> destination);
}