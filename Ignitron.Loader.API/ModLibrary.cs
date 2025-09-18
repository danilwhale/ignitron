using Ignitron.Loader.API.Hacks;

namespace Ignitron.Loader.API;

public static class ModLibrary
{
    public static IReadOnlyList<Mod> Mods => _mods;
    private static readonly List<Mod> _mods = [new AllumeriaMod()];

    public static event Action<Mod>? Loaded;

    public static Mod? FirstOrDefault(string id)
    {
        return _mods.FirstOrDefault(m => m.Metadata.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
    }

    public static Mod? FirstOrDefault(Func<Mod, bool> predicate)
    {
        return _mods.FirstOrDefault(predicate);
    }
    
    public static void Add(Mod mod)
    {
        string id = mod.Metadata.Id;
        if (_mods.Any(m => m.Metadata.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase))) return;
        
        _mods.Add(mod);
        // mod.Initialize(); don't initialize *yet*, mod may access stuff that requires its dependencies
        Loaded?.Invoke(mod);
        
        ModMetadata metadata = mod.Metadata;
        Logger.Init($"Loaded {metadata.Name} (id: {metadata.Id}, version: {metadata.Version})");
    }
}