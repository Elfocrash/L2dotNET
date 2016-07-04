using L2dotNET.Models;

namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServAcceptPlayer : ReceiveAuthPacket
    {
        private string _account;

        public LoginServAcceptPlayer(AuthThread login, byte[] db)
        {
            Makeme(login, db);
        }

        public override void Read()
        {
            _account = ReadS();
        }

        public override void Run()
        {
            AccountModel ta = new AccountModel();
            ta.Login = _account;

            AuthThread.Instance.AwaitAccount(ta);
        }
    }
}