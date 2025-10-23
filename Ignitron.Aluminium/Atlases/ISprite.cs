namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Represents a sprite that can store pixel data
/// </summary>
public interface ISprite
{
    /// <summary>
    /// Width of the sprite
    /// </summary>
    int Width { get; }
    
    /// <summary>
    /// Height of the sprite
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Copies the row of the sprite into destination span
    /// </summary>
    /// <param name="rowIndex">Index of the row to copy</param>
    /// <param name="destination">The span to copy the row to</param>
    void CopyRowTo(int rowIndex, Span<byte> destination);
}