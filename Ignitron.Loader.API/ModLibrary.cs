using Allumeria;

namespace Ignitron.Loader.API;

public static class ModLibrary
{
    public static IReadOnlyList<Mod> Mods => _mods;
    private static readonly List<Mod> _mods = [new AllumeriaMod()];

    public static event Action<Mod>? Loaded;

    public static Mod? FirstOrDefault(string id)
    {
        foreach (Mod mod in _mods)
        {
            if (mod.Metadata.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
            {
                return mod;
            }
        }

        return null;
    }

    public static Mod? FirstOrDefault(Func<Mod, bool> predicate)
    {
        foreach (Mod mod in _mods)
        {
            if (predicate(mod))
            {
                return mod;
            }
        }

        return null;
    }
    
    public static void Add(Mod mod)
    {
        string id = mod.Metadata.Id;
        foreach (Mod other in _mods)
        {
            if (other.Metadata.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Tried to add mod with same ID: {id}");
            }
        }
        
        _mods.Add(mod);
        // mod.Initialize(); don't initialize *yet*, mod may access stuff that requires its dependencies
        Loaded?.Invoke(mod);
        
        ModMetadata metadata = mod.Metadata;
        Logger.Init($"Loaded {metadata.Name} (id: {metadata.Id}, version: {metadata.Version})");
    }
}