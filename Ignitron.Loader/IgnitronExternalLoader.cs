using Allumeria;
using Ignitron.Loader.API;

namespace Ignitron.Loader;

public class IgnitronExternalLoader : IExternalLoader
{
    public void Init()
    {
        Logger.Init("testicular tortion");

        // get version field from loaded assembly
        string fullVersion = Game.VERSION;
        Logger.Init($"Game version: {fullVersion}");

        // append '/ignitron {ver}' so you can identify presence of the modloader
        Game.VERSION = fullVersion + $"/ignitron {ModLoader.Version}";

        // get just version from full version (game stage + version)
        Version version = Version.Parse(fullVersion.AsSpan(fullVersion.IndexOfAny(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'])));
        if (version.Revision < 0) version = new Version(version.Major, version.Minor, version.Build, 0);
        ModLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), "mods"), version);
    }
}