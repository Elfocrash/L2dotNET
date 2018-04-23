﻿using System;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class AuthGameGuard : PacketBase
    {
        private readonly LoginClient _client;
        private readonly int _sessionId;

        public AuthGameGuard(IServiceProvider serviceProvider, Packet p, LoginClient client) : base(serviceProvider)
        {
            _client = client;
            _sessionId = p.ReadInt();
        }

        public override void RunImpl()
        {
            if (_client.State != LoginClientState.Connected)
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
                return;
            }

            if (_sessionId == _client.SessionId)
            {
                _client.State = LoginClientState.AuthedGG;
                _client.Send(GGAuth.ToPacket(_client));
            }
            else
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
            }
        }
    }
}