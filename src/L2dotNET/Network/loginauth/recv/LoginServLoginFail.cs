using System;
using System.Threading.Tasks;

namespace L2dotNET.Network.loginauth.recv
{
    class LoginServLoginFail : PacketBase
    {
        private readonly AuthThread _authThread;
        private readonly string _code;

        public LoginServLoginFail(IServiceProvider serviceProvider, Packet p, AuthThread authThread) : base(serviceProvider)
        {
            _authThread = authThread;
            _code = p.ReadString();
        }

        public override async Task RunImpl()
        {
            _authThread.LoginFail(_code);
        }
    }
}