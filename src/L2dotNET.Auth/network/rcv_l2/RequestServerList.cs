using L2dotNET.Auth.serverpackets;

namespace L2dotNET.Auth.rcv_l2
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
                getClient().sendPacket(new SM_LOGIN_FAIL(getClient(), SM_LOGIN_FAIL.LoginFailReason.ACCESS_FAILED_TRY_AGAIN));
                return;
            }

            getClient().sendPacket(new ServerList(getClient()));
        }
    }
}
