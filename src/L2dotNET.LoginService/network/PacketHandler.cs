using System;
using System.Collections.Concurrent;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Network.InnerNetwork.RequestPackets;
using L2dotNET.LoginService.Network.OuterNetwork.RequestPackets;
using L2dotNET.Network;
using NLog;

namespace L2dotNET.LoginService.Network
{
    public class PacketHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly ConcurrentDictionary<byte, Type> ClientPackets = new ConcurrentDictionary<byte, Type>();
        private static readonly ConcurrentDictionary<byte, Type> ServerPackets = new ConcurrentDictionary<byte, Type>();
        private readonly IServiceProvider _serviceProvider;

        public PacketHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ClientPackets.TryAdd(0x00, typeof(RequestAuthLogin));
            ClientPackets.TryAdd(0x02, typeof(RequestServerLogin));
            ClientPackets.TryAdd(0x05, typeof(RequestServerList));
            ClientPackets.TryAdd(0x07, typeof(AuthGameGuard));

            ServerPackets.TryAdd(0xA0, typeof(RequestLoginServPing));
            ServerPackets.TryAdd(0xA1, typeof(RequestLoginAuth));
            ServerPackets.TryAdd(0xA2, typeof(RequestPlayerInGame));
            ServerPackets.TryAdd(0xA3, typeof(RequestPlayersOnline));
        }

        public void Handle(Packet packet, LoginClient client)
        {
            Log.Debug($"Received packet with Opcode:{packet.FirstOpcode:X2} for State:{client.State}");

            if (!ClientPackets.ContainsKey(packet.FirstOpcode))
            {
                Log.Debug("Unknown packet opcode!");
                return;
            }

            PacketBase incPacket = (PacketBase)Activator.CreateInstance(ClientPackets[packet.FirstOpcode], _serviceProvider, packet, client);
            incPacket?.RunImpl();
        }

        public void Handle(Packet packet, ServerThread client)
        {
            Log.Debug($"Received packet with Opcode:{packet.FirstOpcode:X2}");

            if (!ServerPackets.ContainsKey(packet.FirstOpcode))
            {
                Log.Debug("Unknown packet opcode!");
                return;
            }

            PacketBase incPacket = (PacketBase)Activator.CreateInstance(ServerPackets[packet.FirstOpcode], _serviceProvider, packet, client);
            incPacket?.RunImpl();
        }
    }
}