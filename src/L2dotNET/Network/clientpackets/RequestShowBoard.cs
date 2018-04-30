using System;
using L2dotNET.Managers.bbs;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.clientpackets
{
    class RequestShowBoard : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _type;
        private readonly BbsManager _bbsManager;

        public RequestShowBoard(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _type = packet.ReadInt();
            _bbsManager = serviceProvider.GetService<BbsManager>();
        }

        public override void RunImpl()
        {
            _bbsManager.RequestShow(_client.CurrentPlayer, _type);
        }
    }
}