using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ignitron.Loader.API;

// TODO: later move back into Loader project
public static partial class ModLoader
{
    public static void Load(string path)
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
            Console.Error.WriteLine($"Failed to retrieve directories from {path}:\n{ex}");
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
                Console.Error.WriteLine($"Failed to process directory {dir}:\n{ex}");
            }
        }
    }

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

    [GeneratedRegex("\\w+")]
    private static partial Regex MetadataIdRegex();
}