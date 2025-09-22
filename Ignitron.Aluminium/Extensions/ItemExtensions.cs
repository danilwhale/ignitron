using Allumeria.Items;
using Ignitron.Aluminium.Atlases.Sprites;

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
}