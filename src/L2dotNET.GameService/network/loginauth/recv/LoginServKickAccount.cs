using L2dotNET.Network;

namespace L2dotNET.GameService.Network.LoginAuth.Recv
{
    class LoginServKickAccount : PacketBase
    {
        private string _account;
        private readonly AuthThread _login;
        public LoginServKickAccount(Packet p, AuthThread login)
        {
            _login = login;
            _account = p.ReadString();
        }

        public override void RunImpl()
        {
            //L2World.Instance.KickAccount(account);
        }
    }
}