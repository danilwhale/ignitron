namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Registries for atlases of common textured game objects, such as blocks and items
/// </summary>
public static class GameAtlases
{
    /// <summary>
    /// Registry backing 'res/textures/terrain.png' atlas
    /// </summary>
    public static TextureSpriteRegistry Blocks { get; internal set; } = null!;

    /// <summary>
    /// Registry backing 'res/textures/items.png' atlas
    /// </summary>
    public static TextureSpriteRegistry Items { get; internal set; } = null!;

    // atlases are initialized through Patches/DrawingPatches.cs
}