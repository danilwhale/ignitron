namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Collection of common game texture atlases (such as blocks, items, particles, etc.)
/// </summary>
public static partial class TextureAtlases
{
    /// <summary>
    /// The texture atlas of block sprites (<c>res/textures/terrain.png</c>)
    /// </summary>
    public static TextureAtlas Blocks { get; private set; } = null!;

    /// <summary>
    /// The texture atlas of block emission sprites (<c>res/textures/terrain_emission.png</c>)
    /// </summary>
    public static TextureAtlas BlocksEmission { get; private set; } = null!;

    /// <summary>
    /// The texture atlas of item sprites (<c>res/textures/items.png</c>)
    /// </summary>
    public static TextureAtlas Items { get; private set; } = null!;

    /// <summary>
    /// The texture atlas of GUI sprites (<c>res/textures/ui.png</c>)
    /// </summary>
    public static TextureAtlas UI { get; private set; } = null!;

    /// <summary>
    /// The texture atlas of particle sprites (<c>res/textures/particle.png</c>)
    /// </summary>
    public static TextureAtlas Particles { get; private set; } = null!;

    /// <summary>
    /// The texture atlas of celestial body sprites (sun and moon) (<c>res/textures/sun_moon.png</c>)
    /// </summary>
    public static TextureAtlas CelestialBodies { get; private set; } = null!;
}