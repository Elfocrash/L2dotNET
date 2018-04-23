using System;
using L2dotNET.Managers.bbs;

namespace L2dotNET.Network.clientpackets
{
    class RequestShowBoard : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _type;

        public RequestShowBoard(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _type = packet.ReadInt();
        }

        public override void RunImpl()
        {
            BbsManager.Instance.RequestShow(_client.CurrentPlayer, _type);
        }
    }
}