using L2dotNET.Models;

namespace L2dotNET.Network.loginauth.recv
{
    class LoginServAcceptPlayer : PacketBase
    {
        private readonly AuthThread _login;
        private readonly string _account;

        public LoginServAcceptPlayer(Packet p, AuthThread login)
        {
            _login = login;
            _account = p.ReadString();
        }

        public override void RunImpl()
        {
            AccountModel ta = new AccountModel
            {
                Login = _account
            };

            AuthThread.Instance.AwaitAccount(ta);
        }
    }
}