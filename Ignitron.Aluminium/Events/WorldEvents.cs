using Allumeria.ChunkManagement;
using Allumeria.EntitySystem.Entities;

namespace Ignitron.Aluminium.Events;

public static partial class WorldEvents
{
    public static event Action<World, PlayerEntity>? Loading;
    public static event Action<World, PlayerEntity>? Loaded;
}