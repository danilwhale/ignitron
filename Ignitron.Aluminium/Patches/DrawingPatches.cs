using System.Reflection;
using Allumeria.Rendering;
using HarmonyLib;
using Ignitron.Aluminium.Atlases;

namespace Ignitron.Aluminium.Patches;

[HarmonyPatch]
internal static class DrawingPatches
{
    [HarmonyPatch]
    private static class InitPatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.DeclaredMethod("Allumeria.Rendering.Drawing:Init");
        }

        private static void Postfix(Texture ___defaultTexture, Texture ___itemTexture)
        {
            GameAtlases.Blocks = new TextureSpriteRegistry(___defaultTexture, false);
            GameAtlases.Items = new TextureSpriteRegistry(___itemTexture, true);
        }
    }
}