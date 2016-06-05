using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class AuthGameGuard
    {
        private readonly LoginClient client;

        public AuthGameGuard(Packet p, LoginClient client)
        {
            this.client = client;
            // do nothing
        }

        public void RunImpl()
        {
            client.Send(GGAuth.ToPacket(client));
        }
    }
}