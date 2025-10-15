using System.Reflection;
using Allumeria.Rendering;
using HarmonyLib;

namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Collection of common game texture atlases (such as blocks, items, particles, etc.)
/// </summary>
public static class TextureAtlases
{
    [HarmonyPatch]
    private static class DrawingPatches
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.DeclaredMethod("Allumeria.Rendering.Drawing:Init");
        }

        private static void Postfix(Texture ___defaultTexture, Texture ___uiTexture, Texture ___itemTexture)
        {
            Blocks = new TextureAtlas(___defaultTexture, false, 16);
            UI = new TextureAtlas(___uiTexture, true, 16);
            Items = new TextureAtlas(___itemTexture, true, 16);
        }
    }

    [HarmonyPatch(typeof(WorldRenderer), nameof(WorldRenderer.Init))]
    private static class WorldRendererPatches
    {
        private static void Postfix(Texture ___sunTexture, Texture ___terrainEmissionTexture, Texture ___particleTexture)
        {
            CelestialBodies = new TextureAtlas(___sunTexture, true, 16);
            BlocksEmission = new TextureAtlas(___terrainEmissionTexture, false, 16);
            Particles = new TextureAtlas(___particleTexture, false, 8);
        }
    }

    /// <summary>
    /// The texture atlas of block sprites (<c>res/textures/terrain.png</c>)
    /// </summary>
    public static TextureAtlas Blocks { get; private set; }

    /// <summary>
    /// The texture atlas of block emission sprites (<c>res/textures/terrain_emission.png</c>)
    /// </summary>
    public static TextureAtlas BlocksEmission { get; private set; }

    /// <summary>
    /// The texture atlas of item sprites (<c>res/textures/items.png</c>)
    /// </summary>
    public static TextureAtlas Items { get; private set; }

    /// <summary>
    /// The texture atlas of GUI sprites (<c>res/textures/ui.png</c>)
    /// </summary>
    public static TextureAtlas UI { get; private set; }

    /// <summary>
    /// The texture atlas of particle sprites (<c>res/textures/particle.png</c>)
    /// </summary>
    public static TextureAtlas Particles { get; private set; }

    /// <summary>
    /// The texture atlas of celestial body sprites (sun and moon) (<c>res/textures/sun_moon.png</c>)
    /// </summary>
    public static TextureAtlas CelestialBodies { get; private set; }
}