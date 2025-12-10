using Allumeria.ChunkManagement;
using Allumeria.EntitySystem;
using Allumeria.EntitySystem.Entities;
using Allumeria.Items;

namespace Ignitron.Aluminium.Events;

public static partial class PlayerEvents
{
    public static event Action<PlayerEntity>? Damaged;
    public static event Action<PlayerEntity, float>? Fell;
    public static event Action<PlayerEntity>? Died; 
    public static event Action<PlayerEntity>? Spawned;
    public static event Action<PlayerEntity, Entity>? Punched; 
    public static event Action<PlayerEntity, ItemStack>? DroppedItem;
    public static event Action<PlayerEntity, int, int, int>? SpawnPointChanged;
    public static event Action<PlayerEntity, World>? QuickStackedToNearbyChests;
}