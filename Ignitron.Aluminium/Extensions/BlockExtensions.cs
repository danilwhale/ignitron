using Allumeria;
using Allumeria.Blocks.BlockModels;
using Allumeria.Blocks.Blocks;
using Ignitron.Aluminium.Atlases;
using OpenTK.Mathematics;

namespace Ignitron.Aluminium.Extensions;

public static class BlockExtensions
{
    /// <summary>
    /// Sets the side and main texture of the block to the target sprite
    /// </summary>
    /// <param name="sprite">The target sprite</param>
    public static Block SetTexture(this Block block, in StitchedSprite sprite)
    {
        block.faceTexture =
            block.sideTexture = new FaceTexture(sprite.U0, sprite.V0, sprite.Width, sprite.Height);
        return block;
    }

    /// <summary>
    /// Sets the side texture of the block to the target sprite
    /// </summary>
    /// <param name="sprite">The target sprite</param>
    public static Block SetSideTexture(this Block block, in StitchedSprite sprite)
    {
        // you will have minor inconvenience if your sprite is beyond 4096 on any axis, but this *technically* shouldn't happen
        block.sideTexture = new FaceTexture(sprite.U0, sprite.V0, sprite.Width, sprite.Height);
        return block;
    }

    /// <summary>
    /// Sets the item sprite of the block to the target sprite
    /// </summary>
    /// <param name="sprite">The target sprite</param>
    public static Block SetItemSprite(this Block block, in StitchedSprite sprite)
    {
        // TODO: find a hack to set uv rectangle of an item
        return block.SetItemSprite(sprite.U0, sprite.V0);
    }
    
    /// <summary>
    /// Expands <see cref="Block.blocks"/> to fit specified count of blocks
    /// </summary>
    /// <param name="count">Count of blocks to expand registry for</param>
    public static void GrowRegistry(int count)
    {
        if (Block.totalBlockCount + count >= Block.blocks.Length)
        {
            int newCapacity = MathHelper.Clamp(Block.totalBlockCount * 2, Block.totalBlockCount + count, Array.MaxLength);
            Array.Resize(ref Block.blocks, newCapacity);
            Logger.Info($"Grew blocks array to {newCapacity}");
        }
    }

    /// <summary>
    /// Expands <see cref="Block.blocks"/> to fit the block and adds it
    /// </summary>
    /// <param name="create">Block creation function</param>
    /// <returns>Block created using <paramref name="create"/></returns>
    public static Block Add(Func<Block> create)
    {
        GrowRegistry(1);
        ItemExtensions.GrowRegistry(1); // grow items registry too, because block adds a new item every time it's registered
        return create();
    }
}