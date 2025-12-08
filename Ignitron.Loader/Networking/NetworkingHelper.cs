namespace Ignitron.Loader.Networking;

internal static class NetworkingHelper
{
    public static void DecodeMods(BinaryReader reader, ref List<ClientModDescription>? destination)
    {
        if (destination == null)
        {
            destination = [];
        }
        else
        {
            destination.Clear();
        }

        int count = reader.ReadInt32();
        for (int i = 0; i < count ; i++)
        {
            string id = reader.ReadString();
            int major = reader.ReadInt32(),
                minor = reader.ReadInt32(),
                build = reader.ReadInt32(),
                revision = reader.ReadInt32();
            destination.Add(new ClientModDescription(
                id,
                build >= 0 && revision >= 0 ? new Version(major, minor, build, revision)
                : build >= 0 ? new Version(major, minor, build)
                : new Version(major, minor)));
        }
    }

    public static void EncodeMods(BinaryWriter writer, List<ClientModDescription> source)
    {
        writer.Write(source.Count);
        foreach (ClientModDescription mod in source)
        {
            writer.Write(mod.Id);
            writer.Write(mod.Version.Major);
            writer.Write(mod.Version.Minor);
            writer.Write(mod.Version.Build);
            writer.Write(mod.Version.Revision);
        }
    }
}