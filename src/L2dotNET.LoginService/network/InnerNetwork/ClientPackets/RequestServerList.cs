using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestServerList
    {
        private readonly int _login1;
        private readonly int _login2;
        private readonly LoginClient _client;

        public RequestServerList(Packet p, LoginClient client)
        {
            _client = client;
            _login1 = p.ReadInt();
            _login2 = p.ReadInt();
        }

        public void RunImpl()
        {
            if ((_client.Login1 != _login1) && (_client.Login2 != _login2))
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                return;
            }

            _client.Send(ServerList.ToPacket(_client));
        }
    }
}