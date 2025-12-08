using Allumeria.ChunkManagement;
using Allumeria.EntitySystem.Entities;
using HarmonyLib;

namespace Ignitron.Aluminium.Events;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
[HarmonyPatch(typeof(World))]
public static partial class WorldEvents
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(World.InitAfterLoad))]
    private static void ImplLoading(World __instance, PlayerEntity playerEntity) => Loading?.Invoke(__instance, playerEntity);
    
    [HarmonyPostfix]
    [HarmonyPatch(nameof(World.InitAfterLoad))]
    private static void ImplLoaded(World __instance, PlayerEntity playerEntity) => Loaded?.Invoke(__instance, playerEntity);
}