using L2dotNET.LoginService.GSCommunication;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestPlayerInGame
    {
        private readonly string account;
        private readonly byte status;
        private ServerThread thread;

        public RequestPlayerInGame(Packet p, ServerThread server)
        {
            this.thread = server;
            account = p.ReadString();
            status = p.ReadByte();
        }

        public void RunImpl()
        {
            thread.AccountInGame(account, status);
        }
    }
}