using System;
using System.Threading.Tasks;
using L2dotNET.Logging.Abstraction;

namespace L2dotNET.Network.loginauth.recv
{
    class LoginServPingResponse : PacketBase
    {
        private readonly AuthThread _authThread;
        private readonly int _key;

        public LoginServPingResponse(IServiceProvider serviceProvider, Packet p, AuthThread authThread) : base(serviceProvider)
        {
            _authThread = authThread;
            _key = p.ReadInt();
        }

        public override async Task RunImpl()
        {
            if (_key != _authThread.RandomPingKey)
            {
                Log.Halt($"Invalid random ping response {_key} != {_authThread.RandomPingKey}");
            }
        }
    }
}