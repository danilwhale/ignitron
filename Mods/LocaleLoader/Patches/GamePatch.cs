using System.Globalization;
using HarmonyLib;
using Allumeria;
using Allumeria.DataManagement.Translation;

namespace LocaleLoader.Patches;

[HarmonyPatch(typeof(Game))]
public static class GamePatch
{
    [HarmonyPatch(nameof(Game.SetupGame))]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> SetupGamePatch(IEnumerable<CodeInstruction> instructions)
    {
        foreach (CodeInstruction instruction in instructions)
        {
            if (instruction.Calls(
                    SymbolExtensions.GetMethodInfo<string>(translation => Translator.LoadTranslation(translation))))
            {
                yield return CodeInstruction.Call<string>(translation => TryLoadNativeTranslation(translation));
                continue;
            }

            yield return instruction;
        }
    }

    public static void TryLoadNativeTranslation(string fallbackCode)
    {
        CultureInfo systemCulture = CultureInfo.InstalledUICulture;
        string code = systemCulture.Name.ToLowerInvariant();
        code = $"{code[..2]}_{code[3..]}";
        string path = Path.Combine(Directory.GetCurrentDirectory(), "res", "translations", code + ".txt");
        if (!File.Exists(path))
        {
            Console.WriteLine("Locale Loader: Game doesn't have translation for {0}! Falling back to {1}", code, fallbackCode);
            Translator.LoadTranslation(fallbackCode);
        }
        else Translator.LoadTranslation(code);
    }
}