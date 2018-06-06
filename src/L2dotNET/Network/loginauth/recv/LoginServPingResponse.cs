using System;
using System.Threading.Tasks;

namespace L2dotNET.Network.loginauth.recv
{
    class LoginServPingResponse : PacketBase
    {
        private readonly AuthThread _login;
        private readonly string _message;

        public LoginServPingResponse(IServiceProvider serviceProvider, Packet p, AuthThread login) : base(serviceProvider)
        {
            _login = login;
            _message = p.ReadString();
        }

        public override async Task RunImpl()
        {
            await Task.FromResult(1);
        }
    }
}