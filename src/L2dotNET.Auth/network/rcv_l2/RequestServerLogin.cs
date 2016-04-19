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
                getClient().sendPacket(new SM_LOGIN_FAIL(getClient(), SM_LOGIN_FAIL.LoginFailReason.SYSTEM_ERROR));
                return;
            }

            L2Server server = ServerThreadPool.getInstance().get(serverId);
            if (server == null)
            {
                getClient().sendPacket(new SM_LOGIN_FAIL(getClient(), SM_LOGIN_FAIL.LoginFailReason.SYSTEM_ERROR));
                return;
            }

            if (server.connected == 0)
            {
                getClient().sendPacket(new SM_LOGIN_FAIL(getClient(), SM_LOGIN_FAIL.LoginFailReason.SERVER_MAINTENANCE));
            }
            else
            {
                if (server.gmonly && getClient().activeAccount.builder == 0)
                {
                    getClient().sendPacket(new SM_LOGIN_FAIL(getClient(), SM_LOGIN_FAIL.LoginFailReason.NO_ACCESS_COUPON));
                    return;
                }

                ServerThreadPool.getInstance().SendPlayer(serverId, getClient(), DateTime.Now.ToLocalTime().ToString());

                SQL_Block sqb = new SQL_Block("accounts");
                sqb.param("serverId", serverId);
                sqb.param("lastlogin", DateTime.Now.ToLocalTime());
                sqb.param("lastAddress", getClient()._address);
                sqb.where("account", getClient().activeAccount.name);
                sqb.sql_update();

                getClient().sendPacket(new PlayOk(getClient()));

                
            }
        }
    }
}
