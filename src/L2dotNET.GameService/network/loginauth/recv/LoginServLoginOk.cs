using L2dotNET.Network;

namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServLoginOk : PacketBase
    {
        private readonly AuthThread _login;
        private readonly string _code;

        public LoginServLoginOk(Packet p, AuthThread login)
        {
            _login = login;
            _code = p.ReadString();
        }

        public override void RunImpl()
        {
            _login.LoginOk(_code);
        }
    }
}