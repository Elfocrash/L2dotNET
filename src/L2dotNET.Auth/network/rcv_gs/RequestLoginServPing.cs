using L2dotNET.LoginService.gscommunication;
using L2dotNET.LoginService.Network.OuterNetwork;

namespace L2dotNET.LoginService.network.rcv_gs
{
    class RequestLoginServPing : ReceiveServerPacket
    {
        private string message;
        public RequestLoginServPing(ServerThread server, byte[] data)
        {
            base.makeme(server, data);
        }

        public override void read()
        {
            message = readS();
        }

        public override void run()
        {
            thread.SendPacket(new LoginServPing());
        }
    }
}
