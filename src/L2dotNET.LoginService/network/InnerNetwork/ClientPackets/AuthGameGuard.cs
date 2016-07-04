using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class AuthGameGuard
    {
        private readonly LoginClient _client;

        public AuthGameGuard(Packet p, LoginClient client)
        {
            this._client = client;
            // do nothing
        }

        public void RunImpl()
        {
            _client.Send(GgAuth.ToPacket(_client));
        }
    }
}