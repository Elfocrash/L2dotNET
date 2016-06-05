using L2dotNET.LoginService.gscommunication;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork
{
    class RequestPlayerInGame
    {
        private readonly string account;
        private readonly byte status;
        private ServerThread thread;

        public RequestPlayerInGame(Packet p, ServerThread server)
        {
            account = p.ReadString();
            status = p.ReadByte();
        }

        public void RunImpl()
        {
            thread.AccountInGame(account, status);
        }
    }
}