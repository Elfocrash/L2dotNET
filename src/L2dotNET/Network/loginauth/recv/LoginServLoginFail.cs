﻿using System;

namespace L2dotNET.Network.loginauth.recv
{
    class LoginServLoginFail : PacketBase
    {
        private readonly AuthThread _login;
        private readonly string _code;

        public LoginServLoginFail(IServiceProvider serviceProvider, Packet p, AuthThread login) : base(serviceProvider)
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