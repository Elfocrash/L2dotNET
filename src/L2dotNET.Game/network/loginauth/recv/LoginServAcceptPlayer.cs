using System;

namespace L2dotNET.Game.network.loginauth.recv
{
    class LoginServAcceptPlayer : ReceiveAuthPacket
    {
        private string account;
        private string type;
        private string timeEnd;
        private bool premium;
        private long points;
        private string timeLogIn;
        public LoginServAcceptPlayer(AuthThread login, byte[] db)
        {
            base.makeme(login, db);
        }

        public override void read()
        {
            account = readS();
            type = readS();
            timeEnd = readS();
            premium = readC() == 1;
            points = readQ();
            timeLogIn = readS();
        }

        public override void run()
        {
            LoginSrvTAccount ta = new LoginSrvTAccount();
            ta.name = account;
            ta.type = type;
            ta.timeEnd = timeEnd;
            ta.premium = premium;
            ta.points = points;
            ta.timeLogIn = DateTime.Parse(timeLogIn);

            AuthThread.getInstance().awaitAccount(ta);
        }
    }

    public class LoginSrvTAccount
    {
        public string name;
        public string type;
        public string timeEnd;
        public bool premium;
        public long points;
        public DateTime timeLogIn;
    }
}
