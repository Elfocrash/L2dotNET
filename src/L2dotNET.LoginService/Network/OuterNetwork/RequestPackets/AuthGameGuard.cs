using System;
using System.Threading.Tasks;
using L2dotNET.LoginService.Network.Enums;
using L2dotNET.LoginService.Network.OuterNetwork.ResponsePackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork.RequestPackets
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

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                if (_client.State != LoginClientState.Connected)
                {
                    _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                    _client.Close();
                    return;
                }

                if (_sessionId == _client.SessionId)
                {
                    _client.State = LoginClientState.AuthedGG;
                    _client.SendAsync(GGAuth.ToPacket(_client));
                }
                else
                {
                    _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                    _client.Close();
                }
            });
        }
    }
}