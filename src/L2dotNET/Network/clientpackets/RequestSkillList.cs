using System;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestSkillList : PacketBase
    {
        private readonly GameClient _client;

        public RequestSkillList(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
        }
    }
}
