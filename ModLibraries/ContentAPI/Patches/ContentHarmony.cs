using System.Runtime.CompilerServices;
using HarmonyLib;

namespace ContentAPI.Patches;

internal static class ContentHarmony
{
    [ModuleInitializer]
    public static void Initialize()
    {
        Harmony harmony = new("danilwhale.ContentAPI");
        harmony.PatchAll();
    }
}