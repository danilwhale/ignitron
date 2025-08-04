using System.Reflection;
using Ignitron.Loader.API;

namespace Ignitron.Loader;

public class Entrypoint
{
    public static void Init()
    {
        Console.WriteLine("testicular tortion");

        // get version field from loaded assembly
        Assembly gameAsm = AppDomain.CurrentDomain.GetAssemblies().First(a => a.FullName != null && a.FullName.AsSpan(0, 12) is "PocketBlocks");
        Type gameType = gameAsm.GetType("PocketBlocks.Game") ?? throw new InvalidOperationException("Couldn't find Game class");
        FieldInfo versionField = gameType.GetField("VERSION") ?? throw new InvalidOperationException("Couldn't find Game.VERSION field");
        string fullVersion = (string)versionField.GetValue(null)!;
        Console.WriteLine("Game version: {0}", fullVersion);
        
        // get just version from full version (game stage + version)
        Version version = Version.Parse(fullVersion.AsSpan(fullVersion.IndexOfAny(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'])));
        if (version.Revision < 0) version = new Version(version.Major, version.Minor, version.Build, 0);
        ModLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), "mods"), version);
    }
}