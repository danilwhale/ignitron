using System.Diagnostics.CodeAnalysis;
using Ignitron.Aluminium.Atlases.Sprites;

namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Sprite source that always returns the same sprite
/// </summary>
public sealed class SingleSpriteSource(ISprite sprite) : ISpriteSource
{
    private readonly ISprite _sprite = sprite;

    public bool TryGetSprite(in SpriteLocation location, [NotNullWhen(true)] out ISprite? sprite)
    {
        sprite = _sprite;
        return true;
    }
}