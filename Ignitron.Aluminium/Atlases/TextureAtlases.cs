using System.Reflection;
using Allumeria.Rendering;
using HarmonyLib;

namespace Ignitron.Aluminium.Atlases;

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
    
    public static TextureAtlas Blocks { get; private set; }
    public static TextureAtlas BlocksEmission { get; private set; }
    public static TextureAtlas Items { get; private set; }
    public static TextureAtlas UI { get; private set; }
    public static TextureAtlas Particles { get; private set; }
    public static TextureAtlas CelestialBodies { get; private set; }
}