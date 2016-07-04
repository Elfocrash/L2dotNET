namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServPingResponse : ReceiveAuthPacket
    {
        private string _message;

        public LoginServPingResponse(AuthThread login, byte[] db)
        {
            Makeme(login, db);
        }

        public override void Read()
        {
            _message = ReadS();
        }

        public override void Run() { }
    }
}