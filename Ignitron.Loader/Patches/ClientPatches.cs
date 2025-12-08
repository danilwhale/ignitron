using System.Reflection;
using System.Reflection.Emit;
using Allumeria.Networking;
using Allumeria.Networking.Packets;
using HarmonyLib;
using Ignitron.Loader.Networking;

namespace Ignitron.Loader.Patches;

[HarmonyPatch]
internal static class ClientPatches
{
    private static readonly FieldInfo NetworkManagerClient = AccessTools.DeclaredField(typeof(NetworkManager), nameof(NetworkManager.client));
    private static readonly MethodInfo ClientSendPacketToServer = AccessTools.DeclaredMethod(typeof(Client), nameof(Client.SendPacketToServer));

    private static readonly ConstructorInfo PacketHandshakeCtor = AccessTools.DeclaredConstructor(typeof(PacketHandshake));

    [HarmonyPatch(typeof(Client), nameof(Client.Start))]
    private static class StartPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchStartForward(
                    new CodeMatch(OpCodes.Ldsfld, NetworkManagerClient),
                    new CodeMatch(OpCodes.Newobj, PacketHandshakeCtor),
                    new CodeMatch(OpCodes.Box, typeof(PacketHandshake)),
                    new CodeMatch(OpCodes.Callvirt, ClientSendPacketToServer))
                .ThrowIfInvalid("couldn't find NetworkManager.client.SendPacketToServer(new PacketHandshake())")
                .Advance()
                .RemoveInstructions(2)
                .Insert(
                    CodeInstruction.Call(() => CreateRequest()),
                    new CodeInstruction(OpCodes.Box, typeof(PacketModdedRequest)))
                .Instructions();
        }

        private static PacketModdedRequest CreateRequest()
        {
            List<ClientModDescription> mods = [];

            foreach (ModBox mod in IgnitronLoader.Instance.Mods)
            {
                mods.Add(new ClientModDescription(mod.Metadata.Id, mod.Metadata.Version));
            }

            return new PacketModdedRequest(mods);
        }
    }
}