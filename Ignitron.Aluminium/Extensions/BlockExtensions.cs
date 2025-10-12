using Allumeria;
using Allumeria.Blocks.BlockModels;
using Allumeria.Blocks.Blocks;
using Ignitron.Aluminium.Atlases;
using OpenTK.Mathematics;

namespace Ignitron.Aluminium.Extensions;

public static class BlockExtensions
{
    /// <summary>
    /// Sets the side and main texture of the block to the target location
    /// </summary>
    /// <param name="location">The target location</param>
    public static Block SetTexture(this Block block, in StitchedSprite sprite)
    {
        block.faceTexture = new FaceTexture(sprite.U0, sprite.V0, sprite.Width, sprite.Height);
        return block;
    }

    /// <summary>
    /// Sets the side texture of the block to the target location
    /// </summary>
    /// <param name="location">The target location</param>
    public static Block SetSideTexture(this Block block, in StitchedSprite sprite)
    {
        // you will have minor inconvenience if your sprite is beyond 4096 on any axis, but this *technically* shouldn't happen
        block.sideTexture = new FaceTexture(sprite.U0, sprite.V0, sprite.Width, sprite.Height);
        return block;
    }

    /// <summary>
    /// Sets the item sprite of the block to the target location
    /// </summary>
    /// <param name="location">The target location</param>
    public static Block SetItemSprite(this Block block, in StitchedSprite sprite)
    {
        // TODO: find a hack to set uv rectangle of an item
        return block.SetItemSprite(sprite.U0, sprite.V0);
    }

    /// <summary>
    /// Expands <see cref="Block.blocks"/> to fit the block and adds it
    /// </summary>
    /// <param name="create">Block creation function</param>
    /// <returns>Block created using <paramref name="create"/></returns>
    public static Block Add(Func<Block> create)
    {
        if (Block.totalBlockCount >= Block.blocks.Length)
        {
            // grow blocks array
            int newCapacity = MathHelper.Clamp(Block.totalBlockCount * 2, Block.totalBlockCount + 1, Array.MaxLength);
            Array.Resize(ref Block.blocks, newCapacity);
            Logger.Info($"Grew blocks array to {newCapacity}");
        }

        return create();
    }
}