using System;

namespace L2dotNET.Network.loginauth.recv
{
    class LoginServKickAccount : PacketBase
    {
        private readonly AuthThread _login;
        private readonly string _account;

        public LoginServKickAccount(IServiceProvider serviceProvider, Packet p, AuthThread login) : base(serviceProvider)
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