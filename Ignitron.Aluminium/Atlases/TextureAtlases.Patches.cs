using Allumeria.Rendering;
using HarmonyLib;

namespace Ignitron.Aluminium.Atlases;

public static partial class TextureAtlases
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Drawing), nameof(Drawing.Init))]
    private static void DrawingInitPostfix(Texture ___defaultTexture, Texture ___uiTexture, Texture ___itemTexture)
    {
        Blocks = new TextureAtlas(___defaultTexture, false, 16);
        UI = new TextureAtlas(___uiTexture, true, 16);
        Items = new TextureAtlas(___itemTexture, true, 16);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldRenderer), nameof(WorldRenderer.Init))]
    private static void WorldRendererInitPostfix(Texture ___sunTexture, Texture ___terrainEmissionTexture, Texture ___particleTexture)
    {
        CelestialBodies = new TextureAtlas(___sunTexture, true, 16);
        BlocksEmission = new TextureAtlas(___terrainEmissionTexture, false, 16);
        Particles = new TextureAtlas(___particleTexture, false, 8);
    }
}