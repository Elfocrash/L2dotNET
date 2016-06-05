using L2dotNET.LoginService.Network.OuterNetwork;

namespace L2dotNET.LoginService.Network.InnerNetwork
{
    class AuthGameGuard : ReceiveBasePacket
    {
        public AuthGameGuard(LoginClient Client, byte[] data)
        {
            base.CreatePacket(Client, data);
        }

        public override void Read()
        {
            // do nothing
        }

        public override void Run()
        {
            Client.Send(GGAuth.ToPacket(Client));
        }
    }
}