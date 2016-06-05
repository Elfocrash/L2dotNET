using L2dotNET.LoginService.gscommunication;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestLoginServPing
    {
        private string message;
        private readonly ServerThread thread;

        public RequestLoginServPing(Packet p, ServerThread server)
        {
            thread = server;
            message = p.ReadString();
        }

        public void RunImpl()
        {
            thread.Send(LoginServPing.ToPacket());
        }
    }
}