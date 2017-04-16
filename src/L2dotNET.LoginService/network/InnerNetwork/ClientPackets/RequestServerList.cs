using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestServerList : PacketBase
    {
        private readonly LoginClient _client;
        private readonly int _loginOkID1;
        private readonly int _loginOkID2;

        public RequestServerList(Packet p, LoginClient client)
        {
            _client = client;
            _loginOkID1 = p.ReadInt();
            _loginOkID2 = p.ReadInt();
        }

        public override void RunImpl()
        {
            if (_client.State != LoginClient.LoginClientState.AuthedLogin)
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
                return;
            }

            if (!_client.Key.CheckLoginOKIdPair(_loginOkID1, _loginOkID2))
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
                return;
            }

            _client.Send(ServerList.ToPacket(_client));
        }
    }
}