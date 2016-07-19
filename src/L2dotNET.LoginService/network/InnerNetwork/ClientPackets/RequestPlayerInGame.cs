using L2dotNET.LoginService.GSCommunication;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestPlayerInGame : PacketBase
    {
        private readonly string _account;
        private readonly byte _status;
        private readonly ServerThread _thread;

        public RequestPlayerInGame(Packet p, ServerThread server)
        {
            _thread = server;
            _account = p.ReadString();
            _status = p.ReadByte();
        }

        public override void RunImpl()
        {
            _thread.AccountInGame(_account, _status);
        }
    }
}