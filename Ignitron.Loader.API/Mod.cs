namespace Ignitron.Loader.API;

public abstract class Mod
{
    public ModMetadata Metadata { get; set; }
    
    public virtual void Initialize() { }
}