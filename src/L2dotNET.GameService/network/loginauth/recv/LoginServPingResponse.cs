namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServPingResponse : ReceiveAuthPacket
    {
        private string message;

        public LoginServPingResponse(AuthThread login, byte[] db)
        {
            base.makeme(login, db);
        }

        public override void read()
        {
            message = readS();
        }

        public override void run() { }
    }
}