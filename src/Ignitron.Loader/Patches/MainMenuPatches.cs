using Allumeria.UI.Menus;
using HarmonyLib;

namespace Ignitron.Loader.Patches;

[HarmonyPatch]
internal static class MainMenuPatches
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Render))]
    private static class RenderPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchStartForward(CodeMatch.Calls(() => string.Concat(null, null, null)))
                .ThrowIfInvalid("couldn't find string.Concat(string, string, string)")
                .InsertAfter(CodeInstruction.Call(() => AppendModCount(null!)))
                .Instructions();
        }

        private static string AppendModCount(string brandingText)
            => $"{brandingText}({IgnitronLoader.Instance.Mods.Count} mods)";
    }
}