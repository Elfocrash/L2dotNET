using System;
using System.Collections.Concurrent;
using L2dotNET.Network.loginauth;
using L2dotNET.Network.loginauth.recv;
using NLog;

namespace L2dotNET.Network
{
    public class GamePacketHandlerAuth
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly ConcurrentDictionary<byte, Type> LoginServerPackets = new ConcurrentDictionary<byte, Type>();
        private readonly IServiceProvider _serviceProvider;

        public GamePacketHandlerAuth(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoginServerPackets.TryAdd(0xA1, typeof(LoginServPingResponse));
            LoginServerPackets.TryAdd(0xA5, typeof(LoginServLoginFail));
            LoginServerPackets.TryAdd(0xA6, typeof(LoginServLoginOk));
            LoginServerPackets.TryAdd(0xA7, typeof(LoginServAcceptPlayer));
            LoginServerPackets.TryAdd(0xA8, typeof(LoginServKickAccount));
        }

        public void HandlePacket(Packet packet, AuthThread login)
        {
            PacketBase packetBase = null;

            Log.Info($"Received packet with Opcode:{packet.FirstOpcode:X2}");

            if (LoginServerPackets.ContainsKey(packet.FirstOpcode))
            {
                packetBase = (PacketBase)Activator.CreateInstance(LoginServerPackets[packet.FirstOpcode], _serviceProvider, packet, login);
            }

            if (packetBase == null)
            {
                throw new ArgumentNullException(nameof(packetBase), $"Packet with opcode: {packet.FirstOpcode:X2} doesn't exist in the dictionary.");
            }

            packetBase.RunImpl();
        }
    }
}