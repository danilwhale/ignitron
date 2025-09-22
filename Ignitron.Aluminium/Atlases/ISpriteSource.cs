using System.Diagnostics.CodeAnalysis;
using Ignitron.Aluminium.Atlases.Sprites;

namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Represents source through which sprites can be obtained by their location
/// </summary>
public interface ISpriteSource
{
    /// <summary>
    /// Tries to obtain a sprite by location inside the source
    /// </summary>
    /// <param name="location">Location of a sprite</param>
    /// <param name="sprite">Sprite data behind specified location</param>
    /// <returns><c>true</c> if <paramref name="sprite"/> was obtained successfully; otherwise, <c>false</c></returns>
    bool TryGetSprite(in SpriteLocation location, [NotNullWhen(true)] out ISprite? sprite);
}