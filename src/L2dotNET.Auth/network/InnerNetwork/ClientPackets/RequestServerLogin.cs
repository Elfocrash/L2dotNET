using System;
using L2dotNET.LoginService.data;
using L2dotNET.LoginService.gscommunication;
using L2dotNET.LoginService.Network.OuterNetwork;

namespace L2dotNET.LoginService.Network.InnerNetwork
{
    class RequestServerLogin : ReceiveBasePacket
    {
        public RequestServerLogin(LoginClient Client, byte[] data)
        {
            base.CreatePacket(Client, data);
        }

        int login1, login2;
        byte serverId;
        public override void Read()
        {
            login1 = ReadInt();
            login2 = ReadInt();
            serverId = ReadByte();
        }

        public override void Run()
        {
            if (Client.login1 != login1 && Client.login2 != login2)
            {
                Client.Send(LoginFail.ToPacket(LoginFailReason.REASON_ACCESS_FAILED));
                return;
            }

            L2Server server = ServerThreadPool.Instance.Get(serverId);
            if (server == null)
            {
                Client.Send(LoginFail.ToPacket(LoginFailReason.REASON_ACCESS_FAILED));
                return;
            }

            if (server.Connected == 0)
            {
                Client.Send(LoginFail.ToPacket(LoginFailReason.REASON_SERVER_MAINTENANCE));
            }
            else
            {
                //ServerThreadPool.Instance.SendPlayer(serverId, Client, DateTime.Now.ToLocalTime().ToString());

                //login updates here

                Client.Send(PlayOk.ToPacket(Client));
                //need to add checks to prevent double logins.
                
            }
        }
    }
}
