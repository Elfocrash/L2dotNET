using System;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.RequestPackets
{
    class RequestPlayerInGame : PacketBase
    {
        private readonly ServerThread _thread;
        private readonly int _accountId;
        private readonly byte _status;

        public RequestPlayerInGame(IServiceProvider serviceProvider, Packet p, ServerThread server) : base(serviceProvider)
        {
            _thread = server;
            _accountId = p.ReadInt();
            _status = p.ReadByte();
        }

        public override async Task RunImpl()
        {
            _thread.AccountInGame(_accountId, _status);
        }
    }
}