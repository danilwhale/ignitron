namespace Ignitron.Aluminium.Atlases;

public interface IAtlas
{
    int Width { get; }
    int Height { get; }
    
    StitchedSprite GetSprite(string name);
    bool TryGetSprite(string name, out StitchedSprite stitchedSprite);
}