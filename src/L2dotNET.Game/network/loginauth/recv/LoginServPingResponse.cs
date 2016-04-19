using L2dotNET.Game.network.loginauth.send;

namespace L2dotNET.Game.network.loginauth.recv
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

        public override void run()
        {
            
        }
    }
}
