using L2dotNET.LoginService.gscommunication;
using L2dotNET.LoginService.Network.OuterNetwork;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork
{
    class RequestServerLogin
    {
        private readonly LoginClient client;
        private readonly int login1;
        private readonly int login2;
        private readonly byte serverId;

        public RequestServerLogin(Packet p, LoginClient client)
        {
            this.client = client;
            login1 = p.ReadInt();
            login2 = p.ReadInt();
            serverId = p.ReadByte();
        }

        public void RunImpl()
        {
            if (client.login1 != login1 && client.login2 != login2)
            {
                client.Send(LoginFail.ToPacket(LoginFailReason.REASON_ACCESS_FAILED));
                return;
            }

            L2Server server = ServerThreadPool.Instance.Get(serverId);
            if (server == null)
            {
                client.Send(LoginFail.ToPacket(LoginFailReason.REASON_ACCESS_FAILED));
                return;
            }

            if (server.Connected == 0)
            {
                client.Send(LoginFail.ToPacket(LoginFailReason.REASON_SERVER_MAINTENANCE));
            }
            else
            {
                //ServerThreadPool.Instance.SendPlayer(serverId, Client, DateTime.Now.ToLocalTime().ToString());

                //login updates here

                client.Send(PlayOk.ToPacket(client));
                //need to add checks to prevent double logins.
            }
        }
    }
}