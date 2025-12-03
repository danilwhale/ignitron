using HarmonyLib;

namespace Ignitron.Aluminium;

public static class AluminiumLibrary
{
    public static readonly Version Version = new(0, 0, 1);

    internal static void Initialise()
    {
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new("danilwhale.Ignitron.Aluminium");
        harmony.PatchAll();
    }
}