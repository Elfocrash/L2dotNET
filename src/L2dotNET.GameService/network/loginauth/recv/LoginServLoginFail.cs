namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServLoginFail : ReceiveAuthPacket
    {
        private string _code;

        public LoginServLoginFail(AuthThread login, byte[] db)
        {
            Makeme(login, db);
        }

        public override void Read()
        {
            _code = ReadS();
        }

        public override void Run()
        {
            Login.LoginFail(_code);
        }
    }
}