namespace L2dotNET.GameService.network.loginauth.recv
{
    class LoginServLoginFail : ReceiveAuthPacket
    {
        private string code;

        public LoginServLoginFail(AuthThread login, byte[] db)
        {
            base.makeme(login, db);
        }

        public override void read()
        {
            code = readS();
        }

        public override void run()
        {
            login.loginFail(code);
        }
    }
}