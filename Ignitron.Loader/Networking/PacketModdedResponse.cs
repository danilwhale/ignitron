using System.Text;
using Allumeria;
using Allumeria.Networking;
using Allumeria.Networking.Packets;
using Allumeria.UI.Menus;

namespace Ignitron.Loader.Networking;

public struct PacketModdedResponse(bool success, List<ClientModDescription>? missingMods) : IPacket
{
    public PlayerConnection sender { get; set; }

    private bool _success = success;
    private List<ClientModDescription>? _missingMods = missingMods;

    public void Decode(BinaryReader reader)
    {
        _success = reader.ReadBoolean();
        if (!_success)
        {
            NetworkingHelper.DecodeMods(reader, ref _missingMods);
        }
    }

    public void Encode(BinaryWriter writer)
    {
        writer.Write(_success);
        if (!_success)
        {
            NetworkingHelper.EncodeMods(writer, _missingMods);
        }
    }

    public void PerformAction()
    {
        if (!NetworkManager.IsClient())
        {
            return;
        }

        if (_success)
        {
            NetworkManager.client.SendPacketToServer(new PacketHandshake());
        }
        else
        {
            StringBuilder sb = new();
            sb.Append("Ignitron handshake failure.\nYou're missing the following mods:");
            foreach (ClientModDescription mod in _missingMods)
            {
                sb.AppendLine($" - {mod.Id} ({mod.Version})");
            }
            
            PauseMenu.LeaveGame();
            NetworkManager.client.Stop();
            Game.menu_mainMenu.show = false;
            Game.menu_kicked.show = true;
            Game.menu_kicked.card_reason.displayText = sb.ToString();
        }
    }
}