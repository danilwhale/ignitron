using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Allumeria;

namespace Ignitron.Loader.API;

// TODO: later move back into Loader project
public static partial class ModLoader
{
    private const string BootStage = "boot";

    public static readonly Version Version = new(0, 3, 0);
    public static Version GameVersion { get; private set; }
    public static readonly Assembly Allumeria = AppDomain.CurrentDomain.GetAssemblies().First(a => a.FullName?.StartsWith("Allumeria") ?? false);

    public static void Load(IProgressDisplay display, ICrashHandler crashHandler, string path, Version gameVersion)
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
            ReportCrash(crashHandler, ex, BootStage, $"Failed to retrieve mods from '{path}'");
            return;
        }

        display.UpdateCategory("Processing mod directories");
        for (int i = 0; i < dirs.Length; i++)
        {
            string dir = dirs[i];

            Logger.Init($"Processing '{dir}'... {i + 1}/{dirs.Length}");
            display.UpdateMessage($"{i + 1}/{dirs.Length}");

            try
            {
                ProcessModDirectory(dir);
            }
            catch (Exception ex)
            {
                ReportCrash(crashHandler, ex, BootStage, $"Failed to process mod directory '{dir}'");
                return;
            }
        }

        // verify dependencies for loaded mods and initialize them
        display.UpdateCategory("Initializing mods");
        for (int i = 0; i < ModLibrary.Count; i++)
        {
            Mod mod = ModLibrary.Mods[i];
            ModMetadata metadata = mod.Metadata;

            Logger.Init($"Initializing '{metadata.Name}'... {i + 1}/{ModLibrary.Count} mods");
            display.UpdateMessage($"{metadata.Name} ({i + 1}/{ModLibrary.Count})");

            try
            {
                CheckDependencies(metadata);
            }
            catch (Exception ex)
            {
                ReportCrash(crashHandler, ex, BootStage, null);
                return;
            }

            try
            {
                mod.Initialize();
            }
            catch (Exception ex)
            {
                ReportCrash(crashHandler, ex, BootStage, $"Failed to initialize '{metadata.Id}'");
                return;
            }
        }
    }

    public static void ReportCrash(ICrashHandler handler, Exception ex, string stage, string? state)
    {
        StringBuilder sb = new();
        sb
            .AppendLine("Allumeria has crashed!!!")
            .AppendLine($"The catastrophe has happened during {stage}")
            .AppendLine();

        // report some info about game install
        sb
            .AppendLine("-- Game")
            .AppendLine($"Allumeria: {Game.VERSION}")
            .AppendLine($"Ignitron: {Version}")
            .AppendLine();

        // report some runtime info
        sb
            .AppendLine("-- Runtime")
            .AppendLine($".NET: {RuntimeEnvironment.GetSystemVersion()}")
            .AppendLine($"OS: {Environment.OSVersion}")
            .AppendLine();

        if (state is not null) sb.AppendLine($"-- {state}:");
        sb.AppendLine(ex.ToString());

        // 1 mod (game) is always present
        // report installed mods
        if (ModLibrary.Count > 1)
        {
            sb
                .AppendLine()
                .AppendLine("-- Mods");
            foreach (Mod mod in ModLibrary.Mods)
            {
                ModMetadata metadata = mod.Metadata;
                sb.AppendLine($" + {metadata.Name} ({metadata.Id}): {metadata.Version}");
            }
        }

        // write report everywhere
        string report = sb.ToString();
        Logger.Error(report);
        Logger.CrashReport(report);
        handler.HandleCrash(ex, state, report);
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