using System;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestPlayersOnline : PacketBase
    {
        private readonly ServerThread _thread;
        private readonly short _cnt;

        public RequestPlayersOnline(IServiceProvider serviceProvider, Packet p, ServerThread server) : base(serviceProvider)
        {
            _thread = server;
            _cnt = p.ReadShort();
        }

        public override void RunImpl()
        {
            _thread.Curp = _cnt;
        }
    }
}