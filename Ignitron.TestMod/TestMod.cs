using Allumeria;
using HarmonyLib;
using Ignitron.Loader;

namespace Ignitron.TestMod;

public sealed class TestMod : IModEntrypoint
{
    public void Main(ModBox box)
    {
        Logger.Info($"mod is installed at {box.RootPath}");
        Harmony harmony = new("danilwhale.Ignitron.TestMod");
        harmony.PatchAll();
    }
}