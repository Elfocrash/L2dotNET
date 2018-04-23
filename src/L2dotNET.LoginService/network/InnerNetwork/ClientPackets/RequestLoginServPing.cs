using System;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestLoginServPing : PacketBase
    {
        private readonly ServerThread _thread;
        private string _message;

        public RequestLoginServPing(IServiceProvider serviceProvider, Packet p, ServerThread server) : base(serviceProvider)
        {
            _thread = server;
            _message = p.ReadString();
        }

        public override void RunImpl()
        {
            _thread.Send(LoginServPing.ToPacket());
        }
    }
}