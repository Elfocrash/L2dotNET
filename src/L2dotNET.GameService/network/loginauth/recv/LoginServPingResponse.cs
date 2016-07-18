using L2dotNET.Network;

namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServPingResponse : PacketBase
    {
        private string _message;
        private readonly AuthThread _login;
        public LoginServPingResponse(Packet p, AuthThread login)
        {
            _login = login;
            _message = p.ReadString();
        }

        public override void RunImpl() { }
    }
}