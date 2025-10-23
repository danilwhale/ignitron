using System.Runtime.CompilerServices;
using Allumeria.Blocks.Blocks;
using Allumeria.Blocks.Structures;
using Allumeria.DataManagement;
using Allumeria.DataManagement.Saving;

namespace Ignitron.Aluminium.Assets;

// field of gross hacks
// 'n' parameter means that it must be null
internal static class UnsafeAccessors
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "palette")]
    public static extern ref BlockPalette GetStructurePalette(Structure s);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "ReadBytes")]
    public static extern void ReadBlockPaletteBytes(BlockPalette p, ListTag tag, PaletteConstructor? paletteConstructor, bool readStrings = false);

    [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "GetFromBytes")]
    public static extern LatchPoint GetLatchPointFromBytes(LatchPoint? n, ListTag latchPointTag);

    [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "GetFromBytes")]
    public static extern Marker GetMarkerFromBytes(Marker? n, ListTag latchPointTag);
}