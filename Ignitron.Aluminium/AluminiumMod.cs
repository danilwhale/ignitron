using HarmonyLib;
using Ignitron.Aluminium.Atlases;
using Ignitron.Aluminium.Events;
using Ignitron.Loader.API;

namespace Ignitron.Aluminium;

public sealed class AluminiumMod : Mod
{
    public override void Initialize()
    {
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new("danilwaffle.Ignitron.Aluminium");
        harmony.PatchAll();
    }
}