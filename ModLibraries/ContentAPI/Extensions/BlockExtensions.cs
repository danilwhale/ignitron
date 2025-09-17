using PocketBlocks.Blocks.Blocks;

namespace ContentAPI.Extensions;

public static class BlockExtensions
{
    public static Block SetIcon(this Block block, Icon? icon)
    {
        if (icon == null) return block;
        return block.SetTexture(icon.Value.SpriteX, icon.Value.SpriteY);
    }
}