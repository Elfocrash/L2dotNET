namespace L2dotNET.GameService.network.loginauth.recv
{
    class LoginServLoginOk : ReceiveAuthPacket
    {
        private string code;

        public LoginServLoginOk(AuthThread login, byte[] db)
        {
            base.makeme(login, db);
        }

        public override void read()
        {
            code = readS();
        }

        public override void run()
        {
            login.loginOk(code);
        }
    }
}