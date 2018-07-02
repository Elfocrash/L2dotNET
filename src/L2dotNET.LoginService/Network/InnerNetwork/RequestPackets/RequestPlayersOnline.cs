using System;
using System.Threading.Tasks;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.RequestPackets
{
    class RequestPlayersOnline : PacketBase
    {
        private readonly ServerThread _thread;
        private readonly short _currentPlayers;

        public RequestPlayersOnline(IServiceProvider serviceProvider, Packet p, ServerThread server) : base(serviceProvider)
        {
            _thread = server;
            _currentPlayers = p.ReadShort();
        }

        public override async Task RunImpl()
        {
            _thread.CurrentPlayers = _currentPlayers;
        }
    }
}