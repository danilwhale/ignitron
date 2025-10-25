using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Allumeria;

namespace Ignitron.Loader;

/// <summary>
/// Represents the core of the mod loader
/// </summary>
public sealed partial class IgnitronLoader : IExternalLoader
{
    /// <summary>
    /// Installed version of the mod loader
    /// </summary>
    public static Version Version { get; } = new(0, 4, 0, 0);

    /// <summary>
    /// Current instance of the mod loader
    /// </summary>
    /// <remarks>
    /// This value is set once <see cref="IExternalLoader.Init"/> implementation has been invoked
    /// </remarks>
    public static IgnitronLoader Instance { get; private set; } = null!;

    [GeneratedRegex(@"(\d+)\.(\d+)(?:\.(\d+))?(?:\.(\d+))?")]
    private static partial Regex VersionRegex();

    /// <summary>
    /// Version of the game installation
    /// </summary>
    public Version GameVersion { get; private set; } = null!;

    /// <summary>
    /// Absolute path to the directory with game executable
    /// </summary>
    public string GamePath { get; private set; } = null!;

    /// <summary>
    /// Absolute path to the directory where mods are installed, that is, "<see cref="GamePath"/>/mods/"
    /// </summary>
    public string ModsPath { get; private set; } = null!;

    /// <summary>
    /// Collections with mods that have been loaded from <see cref="ModsPath"/>
    /// </summary>
    public IReadOnlyList<ModBox> Mods => _mods;

    private List<ModBox> _mods = null!;

    /// <summary>
    /// Determines whether a mod is loaded
    /// </summary>
    /// <param name="id">ID of a mod to locate</param>
    /// <returns>true if mod is loaded; otherwise, false</returns>
    public bool IsModLoaded(ReadOnlySpan<char> id)
    {
        foreach (ModBox m in _mods)
        {
            if (m.Metadata.Id.AsSpan().Equals(id, StringComparison.Ordinal))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the mod with the specified unique ID
    /// </summary>
    /// <param name="id">Unique ID of the mod</param>
    /// <param name="mod">When this method returns, contains the mod with the specified unique ID, if the unique ID is found; otherwise, null</param>
    /// <returns>true if the mod was found; otherwise, false</returns>
    public bool TryGetMod(ReadOnlySpan<char> id, [NotNullWhen(true)] out ModBox? mod)
    {
        foreach (ModBox m in _mods)
        {
            if (!m.Metadata.Id.AsSpan().Equals(id, StringComparison.Ordinal)) continue;
            mod = m;
            return true;
        }

        mod = null;
        return false;
    }

    void IExternalLoader.Init()
    {
        try
        {
            Instance = this;

            // resolve game version
            Match versionMatch = VersionRegex().Match(Game.VERSION);
            GameVersion = new Version(
                int.Parse(versionMatch.Groups[1].ValueSpan),
                int.Parse(versionMatch.Groups[2].ValueSpan),
                versionMatch.Groups.Count > 5 ? int.Parse(versionMatch.Groups[3].ValueSpan) : 0,
                versionMatch.Groups.Count > 6 ? int.Parse(versionMatch.Groups[4].ValueSpan) : 0
            );

            GamePath = Directory.GetCurrentDirectory(); // working directory should be the game directory
            ModsPath = Path.Join(GamePath, "mods");

            _mods = new ModResolver(this).Resolve(ModsPath);
            InitialiseMods();

            Logger.Init("Loaded mod(s):");
            foreach (ModBox mod in _mods)
            {
                Logger.Init($" - {mod.Metadata.Id} {mod.Metadata.Version} ({mod.AssemblyPath ?? mod.RootPath})");
            }
        }
        catch (Exception ex)
        {
            // manually handle exception and exit, otherwise the game will continue to initialise
            Logger.Error($"Failed to initialize Ignitron:\n{ex}");
            Logger.CrashReport(ex.ToString());

            // rethrow exception if we're running under debugger, i.e. through ide
            if (!Debugger.IsAttached)
                Environment.Exit(1);
            else
                throw;
        }
    }

    private void InitialiseMods()
    {
        foreach (ModBox mod in _mods)
        {
            try
            {
                List<string> entrypointNames = mod.Metadata.Entrypoints.ToList();
                if (entrypointNames.Count == 0)
                {
                    Logger.Init($"'{mod.Metadata.Id}' doesn't have any entrypoints defined, skipping");
                    continue;
                }

                using ModAssemblyLoadContext ctx = new(mod);
                Assembly ass;
                if (mod.AssemblyPath == null)
                {
                    // load from archive
                    ZipArchiveEntry assemblyEntry = ctx.Archive!.GetEntry(mod.Metadata.AssemblyRelativePath)!;
                    
                    // now we need to decompress the assembly
                    using Stream entryStream = assemblyEntry.Open();
                    using MemoryStream decompStream = new();
                    entryStream.CopyTo(decompStream);
                    decompStream.Position = 0;
                    
                    // finally we can load it
                    ass = ctx.LoadFromStream(decompStream);
                }
                else
                {
                    // load from directory
                    ass = ctx.LoadFromAssemblyPath(mod.AssemblyPath);
                }

                IEnumerable<IModEntrypoint> entrypoints = entrypointNames
                    .Select(e => ass.GetType(e, true)!)
                    .Select(t => (IModEntrypoint)Activator.CreateInstance(t)!);

                foreach (IModEntrypoint entrypoint in entrypoints)
                {
                    entrypoint.Main(mod);
                }
            }
            catch (Exception ex)
            {
                throw new ModInitialiseException($"Unexpected exception when initialising '{mod.Metadata.Id}'", ex);
            }
        }
    }
}