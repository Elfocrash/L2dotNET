namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServLoginOk : ReceiveAuthPacket
    {
        private string _code;

        public LoginServLoginOk(AuthThread login, byte[] db)
        {
            Makeme(login, db);
        }

        public override void Read()
        {
            _code = ReadS();
        }

        public override void Run()
        {
            Login.LoginOk(_code);
        }
    }
}