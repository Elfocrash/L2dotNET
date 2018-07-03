using System;
using System.Threading.Tasks;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Network.InnerNetwork.ResponsePackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.RequestPackets
{
    class RequestLoginServPing : PacketBase
    {
        private readonly ServerThread _thread;
        private int _randomKey;

        public RequestLoginServPing(IServiceProvider serviceProvider, Packet p, ServerThread server) : base(serviceProvider)
        {
            _thread = server;
            _randomKey = p.ReadInt();
        }

        public override async Task RunImpl()
        {
            _thread.Send(LoginServPing.ToPacket(_randomKey));
        }
    }
}