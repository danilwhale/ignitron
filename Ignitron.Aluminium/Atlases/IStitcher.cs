namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Represents an atlas stitcher that can store sprites and copy them to the texture
/// </summary>
public interface IStitcher
{
    /// <summary>
    /// Stitches the given sprite to the texture and adds it to the storage
    /// </summary>
    /// <param name="name">Unique name of the given sprite</param>
    /// <param name="sprite">The sprite to stitch and store</param>
    /// <returns>Pixel bounds of the sprite in the texture</returns>
    StitchedSprite AddSprite(string name, ISprite sprite);

    /// <summary>
    /// Tries to stitch the given sprite to the texture and adds it to the storage
    /// </summary>
    /// <param name="name">Unique name of the given sprite</param>
    /// <param name="sprite">The sprite to stitch and store</param>
    /// <param name="stitchedSprite">Pixel bounds of the sprite in the texture</param>
    /// <returns>A value indicating whether the stitching was successful</returns>
    bool TryAddSprite(string name, ISprite sprite, out StitchedSprite stitchedSprite);
    // void Stitch();
}