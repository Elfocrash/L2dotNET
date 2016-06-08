namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServKickAccount : ReceiveAuthPacket
    {
        private string account;

        public LoginServKickAccount(AuthThread login, byte[] db)
        {
            makeme(login, db);
        }

        public override void read()
        {
            account = readS();
        }

        public override void run()
        {
            //L2World.Instance.KickAccount(account);
        }
    }
}