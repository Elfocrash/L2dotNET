using L2dotNET.LoginService.gscommunication;

namespace L2dotNET.LoginService.Network.InnerNetwork
{
    class RequestPlayerInGame : ReceiveServerPacket
    {
        private string account;
        private byte status;

        public RequestPlayerInGame(ServerThread server, byte[] data)
        {
            base.CreatePacket(server, data);
        }

        public override void read()
        {
            account = ReadString();
            status = ReadByte();
        }

        public override void run()
        {
            thread.AccountInGame(account, status);
        }
    }
}