namespace L2dotNET.Network.loginauth.recv
{
    class LoginServKickAccount : PacketBase
    {
        private readonly AuthThread _login;
        private readonly string _account;

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