using HarmonyLib;
using Ignitron.Loader.API;

namespace LocaleLoader;

public sealed class LocaleLoaderMod : Mod
{
    public override void Initialize()
    {
        Harmony harmony = new($"{Metadata.Author}.{Metadata.Name}");
        harmony.PatchAll();
    }
}