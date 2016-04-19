using L2dotNET.Game.world;

namespace L2dotNET.Game.network.loginauth.recv
{
    class LoginServKickAccount : ReceiveAuthPacket
    {
        private string account;
        public LoginServKickAccount(AuthThread login, byte[] db)
        {
            base.makeme(login, db);
        }

        public override void read()
        {
            account = readS();
        }

        public override void run()
        {
            L2World.getInstance().KickAccount(account);
        }
    }
}
