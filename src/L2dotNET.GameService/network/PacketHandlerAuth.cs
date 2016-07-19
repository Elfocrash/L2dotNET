using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using log4net;
using L2dotNET.GameService.Network.LoginAuth;
using L2dotNET.GameService.Network.LoginAuth.Recv;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network
{
    [Synchronization]
    public class PacketHandlerAuth
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PacketHandlerAuth));

        private static readonly ConcurrentDictionary<byte, Type> ClientPackets = new ConcurrentDictionary<byte, Type>();

        static PacketHandlerAuth()
        {
            ClientPackets.TryAdd(0xA1, typeof(LoginServPingResponse));
            ClientPackets.TryAdd(0xA5, typeof(LoginServLoginFail));
            ClientPackets.TryAdd(0xA6, typeof(LoginServLoginOk));
            ClientPackets.TryAdd(0xA7, typeof(LoginServAcceptPlayer));
            ClientPackets.TryAdd(0xA8, typeof(LoginServKickAccount));
        }

        public static void HandlePacket(Packet packet, AuthThread login)
        {
            PacketBase packetBase = null;

            if (ClientPackets.ContainsKey(packet.FirstOpcode))
                packetBase = ((PacketBase)Activator.CreateInstance(ClientPackets[packet.FirstOpcode], packet, login));

            if (packetBase != null)
                new Thread(packetBase.RunImpl).Start();
        }
    }
}