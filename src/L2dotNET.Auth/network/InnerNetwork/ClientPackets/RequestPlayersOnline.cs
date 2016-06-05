using L2dotNET.LoginService.GSCommunication;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestPlayersOnline
    {
        private readonly short cnt;
        private ServerThread thread;

        public RequestPlayersOnline(Packet p, ServerThread server)
        {
            cnt = p.ReadShort();
        }

        public void RunImpl()
        {
            thread.Curp = cnt;
        }
    }
}