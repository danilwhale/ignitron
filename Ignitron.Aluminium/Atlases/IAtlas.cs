namespace Ignitron.Aluminium.Atlases;

public interface IAtlas
{
    StitchedSprite GetSprite(string name);
    bool TryGetSprite(string name, out StitchedSprite stitchedSprite);
}