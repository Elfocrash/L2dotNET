using L2dotNET.Auth.gscommunication;
using L2dotNET.Auth.network.serverpackets_gs;

namespace L2dotNET.Auth.network.rcv_gs
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
