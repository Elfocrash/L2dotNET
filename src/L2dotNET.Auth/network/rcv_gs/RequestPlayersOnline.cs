using L2dotNET.LoginService.gscommunication;

namespace L2dotNET.LoginService.network.rcv_gs
{
    class RequestPlayersOnline : ReceiveServerPacket
    {
        private short cnt;
        public RequestPlayersOnline(ServerThread server, byte[] data)
        {
            base.makeme(server, data);
        }

        public override void read()
        {
            cnt = readH();
        }

        public override void run()
        {
            thread.Curp = cnt;
        }
    }
}
