using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class AuthGameGuard
    {
        private readonly LoginClient _client;

        public AuthGameGuard(LoginClient client)
        {
            _client = client;
            // do nothing
        }

        public void RunImpl()
        {
            _client.Send(GgAuth.ToPacket(_client));
        }
    }
}