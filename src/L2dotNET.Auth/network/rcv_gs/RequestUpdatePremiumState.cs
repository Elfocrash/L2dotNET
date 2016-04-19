using L2dotNET.Auth.data;
using L2dotNET.Auth.gscommunication;

namespace L2dotNET.Auth.network.rcv_gs
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
            AccountManager.getInstance().UpdatePremium(account, status, points);
        }
    }
}
