using Allumeria.ChunkManagement;
using Allumeria.EntitySystem;
using Allumeria.EntitySystem.Entities;
using Allumeria.Items;
using HarmonyLib;

namespace Ignitron.Aluminium.Events;

[HarmonyPatch(typeof(PlayerEntity))]
public static partial class PlayerEvents
{
    [HarmonyPostfix]
    [HarmonyPatch("OnTakeDamage")]
    private static void ImplDamaged(PlayerEntity __instance) => Damaged?.Invoke(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.TakeFallDamage))]
    private static void ImplFell(PlayerEntity __instance, float distance) => Fell?.Invoke(__instance, distance);
    
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.Die))]
    private static void ImplDied(PlayerEntity __instance) => Died?.Invoke(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.Respawn), typeof(bool), typeof(bool))]
    private static void ImplSpawned(PlayerEntity __instance) => Spawned?.Invoke(__instance);
    
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.DamageEntity))]
    private static void ImplPunched(PlayerEntity __instance, Entity punchedEntity) => Punched?.Invoke(__instance, punchedEntity);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.DropItem))]
    private static void ImplDroppedItem(PlayerEntity __instance, ItemStack? stack)
    {
        if (stack != null) DroppedItem?.Invoke(__instance, stack);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.SetSpawnPoint))]
    private static void ImplSpawnPointChanged(PlayerEntity __instance, int x, int v, int z) => SpawnPointChanged?.Invoke(__instance, x, v, z);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.QuickStackToNearbyChests))]
    private static void ImplQuickStackedToNearbyChests(PlayerEntity __instance, World world) => QuickStackedToNearbyChests?.Invoke(__instance, world);
}