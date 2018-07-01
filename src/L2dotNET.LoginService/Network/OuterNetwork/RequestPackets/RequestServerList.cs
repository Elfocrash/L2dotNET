using System;
using System.Threading.Tasks;
using L2dotNET.LoginService.Network.Enums;
using L2dotNET.LoginService.Network.OuterNetwork.ResponsePackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork.RequestPackets
{
    class RequestServerList : PacketBase
    {
        private readonly LoginClient _client;
        private readonly int _loginOkID1;
        private readonly int _loginOkID2;

        public RequestServerList(IServiceProvider serviceProvider, Packet p, LoginClient client) : base(serviceProvider)
        {
            _client = client;
            _loginOkID1 = p.ReadInt();
            _loginOkID2 = p.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                if (_client.State != LoginClientState.AuthedLogin)
                {
                    _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                    _client.Close();
                    return;
                }

                if (_client.Key.LoginOkId1 != _loginOkID1 || _client.Key.LoginOkId2 != _loginOkID2)
                {
                    _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                    _client.Close();
                    return;
                }

                _client.SendAsync(ServerList.ToPacket(_client));
            });
        }
    }
}