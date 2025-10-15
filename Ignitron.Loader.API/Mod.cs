namespace Ignitron.Loader.API;

public abstract class Mod
{
    public ModMetadata Metadata { get; internal set; }
    public ModRuntimeData RuntimeData { get; internal set; }
    
    public virtual void Initialize() { }
}