using System;
using System.Collections.Concurrent;
using System.Threading;
using log4net;
using L2dotNET.LoginService.Network.InnerNetwork.ClientPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network
{
    public class PacketHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PacketHandler));

        private static readonly ConcurrentDictionary<byte, Type> ClientPackets = new ConcurrentDictionary<byte, Type>();

        static PacketHandler()
        {
            ClientPackets.TryAdd(0x00, typeof(RequestAuthLogin));
            ClientPackets.TryAdd(0x02, typeof(RequestServerLogin));
            ClientPackets.TryAdd(0x05, typeof(RequestServerList));
            ClientPackets.TryAdd(0x07, typeof(AuthGameGuard));
        }

        public static void Handle(Packet packet, LoginClient client)
        {
            PacketBase incPacket = null;
            if (ClientPackets.ContainsKey(packet.FirstOpcode))
                incPacket = ((PacketBase) Activator.CreateInstance(ClientPackets[packet.FirstOpcode], packet, client));

            if (incPacket != null)
                new Thread(incPacket.RunImpl).Start();
        }
    }
}