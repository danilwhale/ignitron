using Allumeria;
using Allumeria.Items;
using Ignitron.Aluminium.Atlases;
using OpenTK.Mathematics;

namespace Ignitron.Aluminium.Extensions;

public static class ItemExtensions
{
    /// <summary>
    /// Creates a new Item using the target sprite location and string ID
    /// </summary>
    /// <param name="location">The target sprite location</param>
    /// <param name="strID">The string ID of an item</param>
    /// <returns></returns>
    public static Item FromSprite(in StitchedSprite sprite, string strID)
    {
        return new Item(sprite.U0, sprite.V0, strID);
    }
    
    /// <summary>
    /// Expands <see cref="Item.items"/> to fit specified count of items
    /// </summary>
    /// <param name="count">Count of items to expand registry for</param>
    public static void GrowRegistry(int count)
    {
        if (Item.totalItemCount + count >= Item.items.Length)
        {
            int newCapacity = MathHelper.Clamp(Item.totalItemCount * 2, Item.totalItemCount + count, Array.MaxLength);
            Array.Resize(ref Item.items, newCapacity);
            Logger.Info($"Grew items array to {newCapacity}");
        }
    }

    /// <summary>
    /// Expands <see cref="Item.items"/> to fit the item and adds it
    /// </summary>
    /// <param name="create">Item creation function</param>
    /// <returns>Item created using <paramref name="create"/></returns>
    public static Item Add(Func<Item> create)
    {
        GrowRegistry(1);
        return create();
    }
}