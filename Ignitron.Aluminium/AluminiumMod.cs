using HarmonyLib;
using Ignitron.Loader;

namespace Ignitron.Aluminium;

internal sealed class AluminiumMod : IModEntrypoint
{
    public void Main(ModBox box)
    {
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new("danilwaffle.Ignitron.Aluminium");
        harmony.PatchAll();
    }
}