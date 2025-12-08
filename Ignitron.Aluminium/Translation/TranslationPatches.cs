using System.Reflection;
using System.Reflection.Emit;
using Allumeria.DataManagement.Translation;
using Allumeria.Settings;
using HarmonyLib;

namespace Ignitron.Aluminium.Translation;

[HarmonyPatch]
internal static class TranslationPatches
{
    private static readonly FieldInfo TranslatorLogMissing = AccessTools.DeclaredField(typeof(Translator), nameof(Translator.logMissing));
    private static readonly FieldInfo GameSettingsCurrentLanguage = AccessTools.DeclaredField(typeof(GameSettings), nameof(GameSettings.current_language));
    private static readonly MethodInfo SettingsEntryGetValue = AccessTools.DeclaredMethod(typeof(SettingsEntry), nameof(SettingsEntry.GetValue));

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Translator), nameof(Translator.LoadTranslation))]
    private static IEnumerable<CodeInstruction> TranslatorLoadTranslationTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            // first we need to load translation keys
            .MatchStartForward(
                // text = (string)GameSettings.current_language.GetValue()
                new CodeMatch(OpCodes.Ldsfld, GameSettingsCurrentLanguage),
                new CodeMatch(OpCodes.Callvirt, SettingsEntryGetValue),
                new CodeMatch(OpCodes.Castclass, typeof(string)),
                new CodeMatch(OpCodes.Stloc_0)
            )
            .ThrowIfInvalid("couldn't find 'text = (string)GameSettings.current_language.GetValue()'")
            .Advance(3)
            .Insert(
                new CodeInstruction(OpCodes.Dup),
                CodeInstruction.Call(() => LoadTranslations(null)))
            // and now we need to translate strings
            .MatchStartForward(
                // if (!Translator.logMissing) {
                new CodeMatch(OpCodes.Ldsfld, TranslatorLogMissing),
                new CodeMatch(OpCodes.Brfalse_S)
            )
            .ThrowIfInvalid("couldn't find 'if (!Translator.logMissing) {'")
            .Insert(
                CodeInstruction.Call(() => Translate()))
            .Instructions();
    }

    private static void LoadTranslations(string locale)
    {
        foreach ((string _, ITranslator translator) in Registries.AluminiumRegistries.Translators)
        {
            translator.Load(Translator.translationKey, locale);
        }
    }

    private static void Translate()
    {
        foreach ((string _, ITranslator translator) in Registries.AluminiumRegistries.Translators)
        {
            translator.Translate();
        }
    }
}