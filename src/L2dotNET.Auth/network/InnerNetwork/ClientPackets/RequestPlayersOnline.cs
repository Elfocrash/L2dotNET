using L2dotNET.LoginService.gscommunication;

namespace L2dotNET.LoginService.Network.InnerNetwork
{
    class RequestPlayersOnline : ReceiveServerPacket
    {
        private short cnt;

        public RequestPlayersOnline(ServerThread server, byte[] data)
        {
            base.CreatePacket(server, data);
        }

        public override void read()
        {
            cnt = ReadShort();
        }

        public override void run()
        {
            thread.Curp = cnt;
        }
    }
}