using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Model;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestServerLogin
    {
        private readonly LoginClient _client;
        private readonly int _login1;
        private readonly int _login2;
        private readonly byte _serverId;

        public RequestServerLogin(Packet p, LoginClient client)
        {
            this._client = client;
            _login1 = p.ReadInt();
            _login2 = p.ReadInt();
            _serverId = p.ReadByte();
        }

        public void RunImpl()
        {
            if ((_client.Login1 != _login1) && (_client.Login2 != _login2))
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                return;
            }

            L2Server server = ServerThreadPool.Instance.Get(_serverId);
            if (server == null)
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                return;
            }

            if (server.Connected == 0)
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonServerMaintenance));
            }
            else
            {
                _client.Send(PlayOk.ToPacket(_client));
            }
        }
    }
}