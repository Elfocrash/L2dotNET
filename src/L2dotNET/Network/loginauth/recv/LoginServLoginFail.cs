namespace L2dotNET.Network.loginauth.recv
{
    class LoginServLoginFail : PacketBase
    {
        private readonly AuthThread _login;
        private readonly string _code;

        public LoginServLoginFail(Packet p, AuthThread login)
        {
            _login = login;
            _code = p.ReadString();
        }

        public override void RunImpl()
        {
            _login.LoginFail(_code);
        }
    }
}