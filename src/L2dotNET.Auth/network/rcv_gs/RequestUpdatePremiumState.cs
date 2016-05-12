using L2dotNET.LoginService.data;
using L2dotNET.LoginService.gscommunication;

namespace L2dotNET.LoginService.network.rcv_gs
{
    class RequestUpdatePremiumState : ReceiveServerPacket
    {
        private string account;
        private byte status;
        private long points;
        public RequestUpdatePremiumState(ServerThread server, byte[] data)
        {
            base.makeme(server, data);
        }

        public override void read()
        {
            account = readS();
            status = readC();
            points = readQ();
        }

        public override void run()
        {
            
        }
    }
}
