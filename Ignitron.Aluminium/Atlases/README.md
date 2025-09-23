# atlas api

simple way to stitch sprites of your mod onto existing game atlas

as of now, it's very primitive, but will be expanded in the future

### usage

```cs
public sealed class MyMod : Mod
{
    public override void Initialize()
    {
        // you can't use TextureSpriteRegistry during LoadedThreaded event as of now
        AllumeriaEvents.Loaded += _ =>
        {
            // you can use AtlasSpriteSource instead of SingleSpriteSource if you have sprites stored
            // in one sheet instead of separate files
            ItemExtensions.Add(() =>
                ItemExtensions.FromSprite(GameAtlases.Items.Register(new SingleSpriteSource(Sprite.FromFile(...)), default), "mymod_drill")
                    .SetStackSize(1)
                    .AddTag(ItemTag.pickaxe, 4)
                    .AddTag(ItemTag.swing_speed, 5)
                    .SetCategory([ItemCategory.technical]));
            BlockExtensions.Add(() =>
                new Block("awa")
                    .MakeSolid()
                    .SetTexture(GameAtlases.Blocks.Register(new SingleSpriteSource(Sprite.FromFile(...)), default))
                    .AutoGenVariants()
                    .SetCategory([ItemCategory.technical]));
            
            // you can use TextureSpriteRegistry.DumpPng to dump runtime contents of the atlas
            // for debugging purposes
            GameAtlases.Items.DumpPng(new ImageWriter(), "items-runtime.png");
            GameAtlases.Blocks.DumpPng(new ImageWriter(), "blocks-runtime.png");
        };
    }
}
```