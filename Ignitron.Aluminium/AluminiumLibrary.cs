using HarmonyLib;

namespace Ignitron.Aluminium;

/// <summary>
/// Represents details about the Aluminium API
/// </summary>
public static class AluminiumLibrary
{
    /// <summary>
    /// Running version of the library 
    /// </summary>
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