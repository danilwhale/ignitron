using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;
using Allumeria;

namespace Ignitron.Loader;

public sealed partial class IgnitronLoader : IExternalLoader
{
    public static Version Version { get; } = new(0, 4, 0, 0);
    public static IgnitronLoader Instance { get; private set; } = null!;

    [GeneratedRegex(@"(\d+)\.(\d+)(?:\.(\d+))?(?:\.(\d+))?")]
    private static partial Regex VersionRegex();

    public Version GameVersion { get; private set; } = null!;
    public string GamePath { get; private set; } = null!;
    public string ModsPath { get; private set; } = null!;
    public IReadOnlyList<ModBox> Mods => _mods;

    private List<ModBox> _mods = null!;

    public bool IsModLoaded(ReadOnlySpan<char> id)
    {
        foreach (ModBox m in _mods)
        {
            if (m.Metadata.Id.AsSpan().Equals(id, StringComparison.Ordinal))
                return true;
        }

        return false;
    }

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
                versionMatch.Groups.Count > 4 ? int.Parse(versionMatch.Groups[3].ValueSpan) : 0,
                versionMatch.Groups.Count > 5 ? int.Parse(versionMatch.Groups[4].ValueSpan) : 0
            );

            GamePath = Directory.GetCurrentDirectory(); // working directory should be the game directory
            ModsPath = Path.Join(GamePath, "mods");

            _mods = new ModResolver(this).Resolve(ModsPath);
            InitialiseMods();
            
            Logger.Init("Loaded mod(s):");
            foreach (ModBox mod in _mods)
            {
                Logger.Init($" - {mod.Metadata.Id} {mod.Metadata.Version} ({mod.AssemblyPath})");
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
                IEnumerable<string>? entrypointNames = mod.Metadata.Entrypoints;
                if (entrypointNames != null && !entrypointNames.Any())
                {
                    Logger.Init($"'{mod.Metadata.Id}' doesn't have any entrypoints defined, skipping");
                    continue;
                }

                Assembly ass = Assembly.LoadFrom(mod.AssemblyPath);

                IEnumerable<IModEntrypoint> entrypoints;
                if (mod.Metadata.Entrypoints != null)
                {
                    // resolve entrypoints from given type names
                    entrypoints = mod.Metadata.Entrypoints
                        .Select(e => ass.GetType(e, true)!)
                        .Where(t => t.IsAssignableTo(typeof(IModEntrypoint)))
                        .Select(t => (IModEntrypoint)Activator.CreateInstance(t)!);
                }
                else
                {
                    // find all entrypoints manually
                    entrypoints = ass.GetExportedTypes()
                        .Where(t => t.IsAssignableTo(typeof(IModEntrypoint)))
                        .Select(t => (IModEntrypoint)Activator.CreateInstance(t)!);
                }

                foreach (IModEntrypoint entrypoint in entrypoints)
                {
                    entrypoint.Main(mod);
                }
            }
            catch (Exception ex)
            {
                throw new ModInitialiseException($"Unexpected exception during initialising '{mod.Metadata.Id}'", ex);
            }
        }
    }
}