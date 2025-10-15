namespace Ignitron.Aluminium.Atlases;

public interface IStitcher
{
    StitchedSprite AddSprite(string name, ISprite sprite);
    bool TryAddSprite(string name, ISprite sprite, out StitchedSprite stitchedSprite);
    // void Stitch();
}