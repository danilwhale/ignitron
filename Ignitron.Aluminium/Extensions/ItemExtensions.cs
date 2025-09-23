using Allumeria;
using Allumeria.Items;
using Ignitron.Aluminium.Atlases.Sprites;
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
    public static Item FromSprite(in SpriteLocation location, string strID)
    {
        return new Item((int)location.AtlasX, (int)location.AtlasY, strID);
    }

    /// <summary>
    /// Expands <see cref="Item.items"/> to fit the item and adds it
    /// </summary>
    /// <param name="create">Item creation function</param>
    /// <returns>Item created using <paramref name="create"/></returns>
    public static Item Add(Func<Item> create)
    {
        if (Item.totalItemCount >= Item.items.Length)
        {
            // grow items array
            int newCapacity = MathHelper.Clamp(Item.totalItemCount * 2, Item.totalItemCount + 1, Array.MaxLength);
            Array.Resize(ref Item.items, newCapacity);
            Logger.Info($"Grew items array to {newCapacity}");
        }

        return create();
    }
}