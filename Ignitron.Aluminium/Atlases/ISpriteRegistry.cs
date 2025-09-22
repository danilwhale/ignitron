using Ignitron.Aluminium.Atlases.Sprites;

namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Represents registry through which sprites can be stitched onto backing atlas
/// </summary>
public interface ISpriteRegistry
{
    /// <summary>
    /// Registers a sprite located at <paramref name="location"/> inside <paramref name="source"/> and returns location of it inside the backing atlas
    /// </summary>
    /// <param name="source">Source of the target sprite</param>
    /// <param name="location">Location of the target sprite relative to its source</param>
    /// <typeparam name="TSource">Type of the sprite source</typeparam>
    /// <returns>Location of a newly registered sprite</returns>
    SpriteLocation Register<TSource>(TSource source, in SpriteLocation location) where TSource : ISpriteSource;
    
    /// <summary>
    /// Tries to register a sprite located at <paramref name="location"/> inside <paramref name="source"/> and returns location of it inside the backing atlas
    /// </summary>
    /// <param name="source">Source of the target sprite</param>
    /// <param name="location">Location of the target sprite relative to its source</param>
    /// <param name="destination">Location of a newly registered sprite</param>
    /// <typeparam name="TSource">Type of the sprite source</typeparam>
    /// <returns><c>true</c> if sprite was registered successfully; otherwise, <c>false</c></returns>
    bool TryRegister<TSource>(TSource source, in SpriteLocation location, out SpriteLocation destination) where TSource : ISpriteSource;
}