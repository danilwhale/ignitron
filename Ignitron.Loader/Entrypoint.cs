using Ignitron.Loader.API;

namespace Ignitron.Loader;

public class Entrypoint
{
    public static void Init()
    {
        Console.WriteLine("testicular tortion");
        ModLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), "mods"));
    }
}