using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestLoginServPing : PacketBase
    {
        private string _message;
        private readonly ServerThread _thread;

        public RequestLoginServPing(Packet p, ServerThread server)
        {
            _thread = server;
            _message = p.ReadString();
        }

        public override void RunImpl()
        {
            _thread.Send(LoginServPing.ToPacket());
        }
    }
}