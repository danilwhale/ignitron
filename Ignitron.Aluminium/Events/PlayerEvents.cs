using Allumeria.ChunkManagement;
using Allumeria.EntitySystem;
using Allumeria.EntitySystem.Entities;
using Allumeria.Items;

namespace Ignitron.Aluminium.Events;

/// <summary>
/// Hooks for common <see cref="PlayerEntity"/> events (such as damaging, falling, dying, etc.)
/// </summary>
public static partial class PlayerEvents
{
    /// <summary>
    /// Occurs when the player gets damaged by something
    /// </summary>
    public static event Action<PlayerEntity>? Damaged;
    
    /// <summary>
    /// Occurs when the <see cref="PlayerEntity"/> falls from the specified distance
    /// </summary>
    public static event Action<PlayerEntity, float>? Fell;
    
    /// <summary>
    /// Occurs when the <see cref="PlayerEntity"/> dies to something
    /// </summary>
    public static event Action<PlayerEntity>? Died;
    
    /// <summary>
    /// Occurs when the <see cref="PlayerEntity"/> respawns (i.e. after dying or loading the world)
    /// </summary>
    public static event Action<PlayerEntity>? Spawned;
    
    /// <summary>
    /// Occurs when the <see cref="PlayerEntity"/> gets damaged by the specified <see cref="Entity"/>
    /// </summary>
    public static event Action<PlayerEntity, Entity>? Punched; 
    
    /// <summary>
    /// Occurs when the <see cref="PlayerEntity"/> drops the specified <see cref="ItemStack"/>
    /// </summary>
    public static event Action<PlayerEntity, ItemStack>? DroppedItem;
    
    /// <summary>
    /// Occurs when the <see cref="PlayerEntity"/> changes its spawn point to the specified XYZ block coordinates
    /// </summary>
    public static event Action<PlayerEntity, int, int, int>? SpawnPointChanged;
    
    /// <summary>
    /// Occurs when the <see cref="PlayerEntity"/> performs quick stack to the nearby chests
    /// </summary>
    public static event Action<PlayerEntity, World>? QuickStackedToNearbyChests;
}