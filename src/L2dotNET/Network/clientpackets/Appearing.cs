using System;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class Appearing : PacketBase
    {
        private readonly GameClient _client;

        public Appearing(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            int x = player.X;
            int y = player.Y;

            if (player.Obsx != -1)
            {
                x = player.Obsx;
                y = player.Obsy;
            }

            player.SendPacket(new UserInfo(player));
            player.ValidateVisibleObjects(x, y, false);
            player.UpdateVisibleStatus();
            

            player.SendActionFailed();
        }
    }
}