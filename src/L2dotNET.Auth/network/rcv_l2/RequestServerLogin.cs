using System;
using L2dotNET.Auth.data;
using L2dotNET.Auth.gscommunication;
using L2dotNET.Auth.serverpackets;

namespace L2dotNET.Auth.rcv_l2
{
    class RequestServerLogin : ReceiveBasePacket
    {
        public RequestServerLogin(LoginClient Client, byte[] data)
        {
            base.makeme(Client, data);
        }

        int login1, login2;
        byte serverId;
        public override void read()
        {
            login1 = readD();
            login2 = readD();
            serverId = readC();
        }

        public override void run()
        {
            if (getClient().login1 != login1 && getClient().login2 != login2)
            {
                getClient().sendPacket(new LoginFail(getClient(), LoginFail.LoginFailReason.REASON_ACCESS_FAILED));
                return;
            }

            L2Server server = ServerThreadPool.Instance.Get(serverId);
            if (server == null)
            {
                getClient().sendPacket(new LoginFail(getClient(), LoginFail.LoginFailReason.REASON_ACCESS_FAILED));
                return;
            }

            if (server.Connected == 0)
            {
                getClient().sendPacket(new LoginFail(getClient(), LoginFail.LoginFailReason.REASON_SERVER_MAINTENANCE));
            }
            else
            {
                ServerThreadPool.Instance.SendPlayer(serverId, getClient(), DateTime.Now.ToLocalTime().ToString());

                //login updates here

                getClient().sendPacket(new PlayOk(getClient()));

                
            }
        }
    }
}
