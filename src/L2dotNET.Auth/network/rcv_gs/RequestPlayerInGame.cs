using L2dotNET.Auth.gscommunication;

namespace L2dotNET.Auth.network.rcv_gs
{
    class RequestPlayerInGame : ReceiveServerPacket
    {
        private string account;
        private byte status;
        public RequestPlayerInGame(ServerThread server, byte[] data)
        {
            base.makeme(server, data);
        }

        public override void read()
        {
            account = readS();
            status = readC();
        }

        public override void run()
        {
            thread.AccountInGame(account, status);
        }
    }
}
