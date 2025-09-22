using Allumeria.Blocks.BlockModels;
using Allumeria.Blocks.Blocks;
using Ignitron.Aluminium.Atlases.Sprites;

namespace Ignitron.Aluminium.Extensions;

public static class BlockExtensions
{
    /// <summary>
    /// Sets the side and main texture of the block to the target location
    /// </summary>
    /// <param name="location">The target location</param>
    public static Block SetTexture(this Block block, in SpriteLocation location)
    {
        return block.SetTexture(location.X, location.Y);
    }

    /// <summary>
    /// Sets the side texture of the block to the target location
    /// </summary>
    /// <param name="location">The target location</param>
    public static Block SetSideTexture(this Block block, in SpriteLocation location)
    {
        // you will have minor inconvenience if your sprite is beyond 4096 on any axis, but this *technically* shouldn't happen
        block.sideTexture = new FaceTexture((ushort)location.AtlasX, (ushort)location.AtlasY, Sprite.SizeInPixels, Sprite.SizeInPixels);
        return block;
    }

    /// <summary>
    /// Sets the item sprite of the block to the target location
    /// </summary>
    /// <param name="location">The target location</param>
    public static Block SetItemSprite(this Block block, in SpriteLocation location)
    {
        return block.SetItemSprite(location.X, location.Y);
    }
}