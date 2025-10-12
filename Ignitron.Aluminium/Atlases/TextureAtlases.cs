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

        private static void Postfix(Texture ___defaultTexture, Texture ___itemTexture)
        {
            Blocks = new TextureAtlas(___defaultTexture, false);
            Items = new TextureAtlas(___itemTexture, true);
        }
    }
    
    public static TextureAtlas Blocks { get; private set; }
    public static TextureAtlas Items { get; private set; }
}