using L2dotNET.LoginService.gscommunication;
using L2dotNET.LoginService.Network.OuterNetwork;

namespace L2dotNET.LoginService.Network.InnerNetwork
{
    class RequestLoginServPing : ReceiveServerPacket
    {
        private string message;
        public RequestLoginServPing(ServerThread server, byte[] data)
        {
            base.CreatePacket(server, data);
        }

        public override void read()
        {
            message = ReadString();
        }

        public override void run()
        {
            thread.Send(LoginServPing.ToPacket());
        }
    }
}
