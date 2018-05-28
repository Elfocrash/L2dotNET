﻿using System;
using System.Collections.Concurrent;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Network.loginauth;
using L2dotNET.Network.loginauth.recv;

namespace L2dotNET.Network
{
    public class GamePacketHandlerAuth
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private static readonly ConcurrentDictionary<byte, Type> ClientPackets = new ConcurrentDictionary<byte, Type>();
        private readonly IServiceProvider _serviceProvider;

        public GamePacketHandlerAuth(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ClientPackets.TryAdd(0xA1, typeof(LoginServPingResponse));
            ClientPackets.TryAdd(0xA5, typeof(LoginServLoginFail));
            ClientPackets.TryAdd(0xA6, typeof(LoginServLoginOk));
            ClientPackets.TryAdd(0xA7, typeof(LoginServAcceptPlayer));
            ClientPackets.TryAdd(0xA8, typeof(LoginServKickAccount));
        }

        public void HandlePacket(Packet packet, AuthThread login)
        {
            PacketBase packetBase = null;
            Log.Info($"Received packet with Opcode:{packet.FirstOpcode:X2}");
            if (ClientPackets.ContainsKey(packet.FirstOpcode))
                packetBase = (PacketBase)Activator.CreateInstance(ClientPackets[packet.FirstOpcode], _serviceProvider, packet, login);

            if (packetBase == null)
                throw new ArgumentNullException(nameof(packetBase), $"Packet with opcode: {packet.FirstOpcode:X2} doesn't exist in the dictionary.");

            packetBase.RunImpl();
        }
    }
}