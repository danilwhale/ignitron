using Allumeria.Networking;
using HarmonyLib;
using Ignitron.Loader.Networking;

namespace Ignitron.Loader.Patches;

[HarmonyPatch]
internal static class NetworkManagerPatches
{
    [HarmonyPatch(typeof(NetworkManager), nameof(NetworkManager.RegisterPackets))]
    private static class RegisterPacketsPatch
    {
        private static void Postfix()
        {
            IPacket.RegisterPacket(new PacketModdedRequest());
            IPacket.RegisterPacket(new PacketModdedResponse());
        }
    }
}