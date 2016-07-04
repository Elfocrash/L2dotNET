namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServKickAccount : ReceiveAuthPacket
    {
        private string _account;

        public LoginServKickAccount(AuthThread login, byte[] db)
        {
            Makeme(login, db);
        }

        public override void Read()
        {
            _account = ReadS();
        }

        public override void Run()
        {
            //L2World.Instance.KickAccount(account);
        }
    }
}