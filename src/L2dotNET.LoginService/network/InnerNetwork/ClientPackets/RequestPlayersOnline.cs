using L2dotNET.LoginService.GSCommunication;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestPlayersOnline
    {
        private readonly short _cnt;
        private readonly ServerThread _thread;

        public RequestPlayersOnline(Packet p, ServerThread server)
        {
            _thread = server;
            _cnt = p.ReadShort();
        }

        public void RunImpl()
        {
            _thread.Curp = _cnt;
        }
    }
}