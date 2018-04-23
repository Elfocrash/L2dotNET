using System;
using L2dotNET.DataContracts;

namespace L2dotNET.Network.loginauth.recv
{
    class LoginServAcceptPlayer : PacketBase
    {
        private readonly AuthThread _login;
        private readonly string _account;

        public LoginServAcceptPlayer(IServiceProvider serviceProvider, Packet p, AuthThread login) : base(serviceProvider)
        {
            _login = login;
            _account = p.ReadString();
        }

        public override void RunImpl()
        {
            AccountContract ta = new AccountContract
            {
                Login = _account
            };

            _login.AwaitAccount(ta);
        }
    }
}