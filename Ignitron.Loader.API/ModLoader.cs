using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Ignitron.Loader.API.Hacks;

namespace Ignitron.Loader.API;

// TODO: later move back into Loader project
public static partial class ModLoader
{
    public static readonly Version Version = new(0, 0, 1);
    public static readonly Assembly Allumeria = AppDomain.CurrentDomain.GetAssemblies().First(a => a.FullName?.StartsWith("Allumeria") ?? false);

    public static void Load(string path, Version gameVersion)
    {
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
                ProcessModDirectory(dir, gameVersion);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to process directory {dir}:\n{ex}");
            }
        }
    }

    private static void ProcessModDirectory(string path, Version gameVersion)
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

        int versionComparison = CompareGameVersions(gameVersion, metadata.GameVersion);
        if (versionComparison != 0)
        {
            if (versionComparison < 0) throw new Exception($"Mod was made for older version of the game (mod: {metadata.GameVersion}, game: {gameVersion})");
            else throw new Exception($"Mod was made for newer version of the game (mod: {metadata.GameVersion}, game: {gameVersion})");
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

    private static int CompareGameVersions(Version installedVersion, ReadOnlySpan<char> targetVersion)
    {
        Span<int> components = [installedVersion.Major, installedVersion.Minor, installedVersion.Build, installedVersion.Revision];

        int i = 0;
        foreach (Range componentRange in targetVersion.Split('.'))
        {
            if (i >= components.Length)
            {
                throw new ArgumentException($"Mod's game version is formatted incorrectly: {targetVersion}. Expected MAJOR.MINOR.BUILD.REVISION!");
            }

            ReadOnlySpan<char> component = targetVersion[componentRange];
            if (component is not ['*']) components[i++] = int.Parse(component);
        }

        return new Version(components[0], components[1], components[2], components[3]).CompareTo(installedVersion);
    }

    [GeneratedRegex("\\w+")]
    private static partial Regex MetadataIdRegex();
}