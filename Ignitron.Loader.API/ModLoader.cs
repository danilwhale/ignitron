using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Allumeria;

namespace Ignitron.Loader.API;

// TODO: later move back into Loader project
public static partial class ModLoader
{
    public static Version Version { get; } = new(0, 3, 0);
    public static Version GameVersion { get; private set; } = new();

    public static void Load(string path, Version gameVersion)
    {
        GameVersion = gameVersion;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            return;
        }

        string[] dirs;
        try
        {
            dirs = Directory.GetDirectories(path);
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to retrieve directories from {path}:\n{ex}");
            return;
        }

        foreach (string dir in dirs)
        {
            try
            {
                ProcessModDirectory(dir);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to process directory {dir}:\n{ex}");
            }
        }

        // verify dependencies for loaded mods and initialize them
        foreach (Mod mod in ModLibrary.Mods)
        {
            ModMetadata metadata = mod.Metadata;

            try
            {
                CheckDependencies(metadata);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                continue;
            }

            try
            {
                mod.Initialize();
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to initialize {metadata.Id}:\n{ex}");
            }
        }
    }

    // load mod from specified directory. this does NOT resolve dependencies! dependencies are resolved in later stage
    private static void ProcessModDirectory(string path)
    {
        ModMetadata? metadata =
            JsonSerializer.Deserialize<ModMetadata>(File.ReadAllText(Path.Combine(path, "Metadata.json")));
        if (metadata == null)
        {
            throw new Exception("Failed to deserialize mod metadata");
        }

        if (!MetadataIdRegex().IsMatch(metadata.Id))
        {
            throw new Exception($"Invalid mod id: '{metadata.Id}', only A-Z, 0-9 and _ are allowed");
        }

        string assemblyPath = Path.Combine(path, metadata.AssemblyFile);
        Assembly asm = Assembly.LoadFrom(assemblyPath);

        IEnumerable<Mod?> mods = asm
            .GetExportedTypes()
            .Where(t => t.IsAssignableTo(typeof(Mod)))
            .Select(t => (Mod?)Activator.CreateInstance(t))
            .Where(m => m != null);
        foreach (Mod? mod in mods)
        {
            mod!.Metadata = metadata;
            ModLibrary.Add(mod);
        }
    }

    // check if all dependencies have been loaded for that mod. call this ONLY after all mods have been processed
    private static void CheckDependencies(ModMetadata metadata)
    {
        foreach (ModDependency dep in metadata.Dependencies)
        {
            Mod? depMod = ModLibrary.FirstOrDefault(dep.Id);
            if (depMod is null)
            {
                if (!dep.Optional)
                {
                    throw new InvalidOperationException($"{metadata.Id} is missing dependency: {dep.Id}");
                }

                continue;
            }

            if (!dep.Version.Equals(depMod.Metadata.Version))
            {
                throw new InvalidOperationException($"{metadata.Id} requires {dep.Id} of version {dep.Version}, but got {depMod.Metadata.Version}");
            }
        }
    }

    [GeneratedRegex("\\w+")]
    private static partial Regex MetadataIdRegex();
}