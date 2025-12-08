using System.Text;
using Allumeria.Networking;
using Allumeria.Networking.Packets;

namespace Ignitron.Loader.Networking;

public struct PacketModdedRequest(List<ClientModDescription> mods) : IPacket
{
    public PlayerConnection sender { get; set; }

    private List<ClientModDescription>? _mods = mods;

    public void Decode(BinaryReader reader)
    {
        NetworkingHelper.DecodeMods(reader, ref _mods);
    }

    public void Encode(BinaryWriter writer)
    {
        if (_mods == null)
        {
            throw new InvalidOperationException();
        }

        NetworkingHelper.EncodeMods(writer, _mods);
    }

    public void PerformAction()
    {
        if (!NetworkManager.IsServer())
        {
            return;
        }

        if (_mods == null)
        {
            throw new InvalidOperationException();
        }

        // verify that all mods are present
        List<ClientModDescription>? missingMods = null;

        foreach (ModBox mod in IgnitronLoader.Instance.Mods)
        {
            bool missed = true;

            foreach (ClientModDescription other in _mods)
            {
                if (other.Id == mod.Metadata.Id && other.Version == mod.Metadata.Version)
                {
                    missed = false;
                    break;
                }
            }

            if (missed)
            {
                missingMods ??= [];
                missingMods.Add(new ClientModDescription(mod.Metadata.Id, mod.Metadata.Version));
            }
        }

        NetworkManager.server.SendPacketTo(new PacketModdedResponse(missingMods == null, missingMods), sender);
    }
}