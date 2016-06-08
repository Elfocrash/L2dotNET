namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServLoginOk : ReceiveAuthPacket
    {
        private string code;

        public LoginServLoginOk(AuthThread login, byte[] db)
        {
            makeme(login, db);
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