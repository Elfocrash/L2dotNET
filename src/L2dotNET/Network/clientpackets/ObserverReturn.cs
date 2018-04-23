using System;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class ObserverReturn : PacketBase
    {
        private readonly GameClient _client;

        public ObserverReturn(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.SendPacket(new ObservationReturn(player.Obsx, player.Obsy, player.Obsz));
        }
    }
}