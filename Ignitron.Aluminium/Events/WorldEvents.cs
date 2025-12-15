using Allumeria.ChunkManagement;
using Allumeria.EntitySystem.Entities;

namespace Ignitron.Aluminium.Events;

/// <summary>
/// Hooks for common <see cref="World"/> events (such as loading)
/// </summary>
public static partial class WorldEvents
{
    /// <summary>
    /// Occurs when the <see cref="World"/> starts loading the <see cref="PlayerEntity"/>
    /// </summary>
    public static event Action<World, PlayerEntity>? Loading;
    
    /// <summary>
    /// Occurs when the <see cref="World"/> has finished loading the <see cref="PlayerEntity"/>
    /// </summary>
    public static event Action<World, PlayerEntity>? Loaded;
}