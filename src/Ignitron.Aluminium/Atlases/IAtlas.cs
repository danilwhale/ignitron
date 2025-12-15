namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Represents an atlas with size and storage of sprites
/// </summary>
public interface IAtlas
{
    /// <summary>
    /// Width of the atlas in pixels
    /// </summary>
    int Width { get; }
    
    /// <summary>
    /// Height of the atlas in pixels
    /// </summary>
    int Height { get; }
    
    /// <summary>
    /// Searches the storage for the sprite with a given name
    /// </summary>
    /// <param name="name">Unique name of the sprite</param>
    /// <returns>Pixel bounds of a sprite in an atlas</returns>
    StitchedSprite GetSprite(string name);
    
    /// <summary>
    /// Searches the storage for the sprite with a given name
    /// </summary>
    /// <param name="name">Unique *nName of the sprite</param>
    /// <param name="stitchedSprite">Pixel bounds of a sprite in an atlas</param>
    /// <returns>A value indicating whether the search was successful</returns>
    bool TryGetSprite(string name, out StitchedSprite stitchedSprite);
}