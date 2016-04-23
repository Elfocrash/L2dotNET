using L2dotNET.Auth.serverpackets;

namespace L2dotNET.Auth.rcv_l2
{
    class AuthGameGuard : ReceiveBasePacket
    {
        public AuthGameGuard(LoginClient Client, byte[] data)
        {
            base.makeme(Client, data);
        }

        public override void read()
        {
            // do nothing
        }

        public override void run()
        {
            this.getClient().sendPacket(new GGAuth(getClient()));
        }
    }
}
