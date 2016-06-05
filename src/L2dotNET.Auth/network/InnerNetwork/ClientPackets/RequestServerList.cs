using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestServerList
    {
        private readonly int login1;
        private readonly int login2;
        private readonly LoginClient client;

        public RequestServerList(Packet p, LoginClient client)
        {
            this.client = client;
            login1 = p.ReadInt();
            login2 = p.ReadInt();
        }

        public void RunImpl()
        {
            if (client.login1 != login1 && client.login2 != login2)
            {
                client.Send(LoginFail.ToPacket(LoginFailReason.REASON_ACCESS_FAILED));
                return;
            }

            client.Send(ServerList.ToPacket(client));
        }
    }
}