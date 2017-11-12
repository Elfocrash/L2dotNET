using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Model;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestServerLogin : PacketBase
    {
        private readonly LoginClient _client;
        private readonly int _loginOkID1;
        private readonly int _loginOkID2;
        private readonly byte _serverId;

        public RequestServerLogin(Packet p, LoginClient client)
        {
            _client = client;
            _loginOkID1 = p.ReadInt();
            _loginOkID2 = p.ReadInt();
            _serverId = p.ReadByte();
        }

        public override void RunImpl()
        {
            if (_client.State != LoginClientState.AuthedLogin)
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
                return;
            }

            if (_client.Key.LoginOkId1 != _loginOkID1 && _client.Key.LoginOkId2 != _loginOkID2)
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
                return;
            }

            L2Server server = ServerThreadPool.Instance.Get(_serverId);
            if (server == null)
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
                return;
            }

            if (server.Connected == 0)
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonServerMaintenance));
                _client.Close();
                return;
            }

            _client.Send(PlayOk.ToPacket(_client));
        }
    }
}