
using L2dotNET.LoginService.Network.OuterNetwork;

namespace L2dotNET.LoginService.rcv_l2
{
    class RequestServerList : ReceiveBasePacket
    {
        public RequestServerList(LoginClient Client, byte[] data)
        {
            base.makeme(Client, data);
        }

        int login1, login2;
        public override void read()
        {
            login1 = readD();
            login2 = readD();
        }

        public override void run()
        {
            if (getClient().login1 != login1 && getClient().login2 != login2)
            {
                getClient().sendPacket(new LoginFail(getClient(), LoginFail.LoginFailReason.REASON_ACCESS_FAILED));
                return;
            }

            getClient().sendPacket(new ServerList(getClient()));
        }
    }
}
