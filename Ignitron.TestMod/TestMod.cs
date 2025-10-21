using Allumeria;
using Ignitron.Loader;

namespace Ignitron.TestMod;

public sealed class TestMod : IModEntrypoint
{
    public void Main(ModBox box)
    {
        Logger.Info($"mod is installed at {box.RootPath}");
    }
}