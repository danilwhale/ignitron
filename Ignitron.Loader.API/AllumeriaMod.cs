namespace Ignitron.Loader.API;

internal sealed class AllumeriaMod : Mod
{
    public AllumeriaMod()
    {
        Metadata = new ModMetadata
        {
            AssemblyFile = string.Empty,
            Id = "allumeria",
            Name = "Allumeria",
            Author = "unomelon",
            Version = ModLoader.GameVersion,
            Description = null,
            Dependencies = []
        };
    }
}