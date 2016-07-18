using L2dotNET.Network;

namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServLoginFail : PacketBase
    {
        private readonly string _code;
        private readonly AuthThread _login;
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