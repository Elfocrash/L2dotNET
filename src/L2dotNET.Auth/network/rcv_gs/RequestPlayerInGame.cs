using L2dotNET.LoginService.gscommunication;

namespace L2dotNET.LoginService.network.rcv_gs
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
