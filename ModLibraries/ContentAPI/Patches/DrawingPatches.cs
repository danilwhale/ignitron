using System.Reflection;
using HarmonyLib;

namespace ContentAPI.Patches;

[HarmonyPatch]
internal static class DrawingPatches
{
    [HarmonyPatch]
    private static class InitPatch
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.DeclaredMethod("PocketBlocks.Rendering.Drawing:Init");
        }

        [HarmonyPostfix]
        public static void Postfix()
        {
            BlockIconRegister.Init();
        }
    }
}