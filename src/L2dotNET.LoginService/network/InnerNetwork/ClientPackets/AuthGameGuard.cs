using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class AuthGameGuard : PacketBase
    {
        private readonly LoginClient _client;

        public AuthGameGuard(Packet p, LoginClient client)
        {
            _client = client;
            // do nothing
        }

        public override void RunImpl()
        {
            _client.Send(GGAuth.ToPacket(_client));
        }
    }
}