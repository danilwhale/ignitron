namespace Ignitron.Loader.API;

public static class ModLibrary
{
    public static IReadOnlyList<Mod> Mods => _mods;
    private static readonly List<Mod> _mods = [];

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
        mod.Initialize();
        Loaded?.Invoke(mod);
        
        ModMetadata metadata = mod.Metadata;
        Console.WriteLine($"Loaded {metadata.Name} (id: {metadata.Id}, version: {metadata.Version})");
    }
}