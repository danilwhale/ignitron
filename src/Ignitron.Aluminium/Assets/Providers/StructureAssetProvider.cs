using System.Runtime.CompilerServices;
using Allumeria.Blocks.Blocks;
using Allumeria.Blocks.Structures;
using Allumeria.DataManagement;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class StructureAssetProvider : IAssetProvider<Structure>
{
    public static StructureAssetProvider Default { get; } = new();

    public Structure Create(AssetManager assets, string assetName)
    {
        Structure structure = new();

        // i'm really sorry for copying that much gross but this is the only way it could've been done
        using Stream input = assets.Open(assetName);
        using BinaryReader binaryReader = new(input);
        _ = binaryReader.ReadByte();
        _ = binaryReader.ReadString();

        ListTag rootTag = new("root");
        rootTag.ReadBytes(binaryReader);

            GetStructurePalette(structure) = new BlockPalette();
        ListTag? infoTag = (ListTag?)rootTag.FindTag("info");
        ListTag? paletteTag = (ListTag?)rootTag.FindTag("palette");

        if (infoTag != null)
        {
            structure.sizeX = (int)infoTag.GetValueOrDefault("w", 0);
            structure.sizeY = (int)infoTag.GetValueOrDefault("h", 0);
            structure.sizeZ = (int)infoTag.GetValueOrDefault("l", 0);
        }

        if (paletteTag != null)
        {
            GetStructurePalette(structure).ReadBytes(paletteTag, null, readStrings: true);
        }

        // ReSharper disable PossibleInvalidCastExceptionInForeachLoop

        if (rootTag.FindTag("latch_points", out DataTag latchPoints))
        {
            foreach (ListTag latchPointTag in ((ListTag)latchPoints).tags.Values)
            {
                LatchPoint latchPoint = LatchPoint.GetFromBytes(latchPointTag);
                structure.latchPoints.Add(latchPoint);
            }
        }

        if (rootTag.FindTag("markers", out DataTag tags))
        {
            foreach (ListTag markerTag in ((ListTag)tags).tags.Values)
            {
                Marker marker = Marker.GetFromBytes(markerTag);
                structure.markers.Add(marker);
            }
        }

        // ReSharper restore PossibleInvalidCastExceptionInForeachLoop

        binaryReader.Close();

        return structure;
        
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "palette")]
        static extern ref BlockPalette GetStructurePalette(Structure s);
    }
}