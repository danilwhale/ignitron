using Allumeria;
using HarmonyLib;
using Ignitron.Loader;

namespace Ignitron.TestMod;

[HarmonyPatch]
internal static class MainMenuPatches
{
    [HarmonyPatch("Allumeria.UI.Menus.MainMenu", "BuildMenu")]
    private static class BuildMenuPatch
    {
        private static void Prefix()
        {
            if (!IgnitronLoader.Instance.TryGetMod("test_mod", out ModBox? mod))
            {
                Logger.Error("No mod?");
                return;
            }

            Logger.Info($"Hello from test mod! Mod is located at {mod.RootPath}");
        }
    }
}