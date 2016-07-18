using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class Appearing : PacketBase
    {
        private readonly GameClient _client;

        public Appearing(Packet packet, GameClient client)
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

            if (player.Summon != null)
            {
                player.Summon.ValidateVisibleObjects(x, y, false);
                player.Summon.IsTeleporting = false;
            }

            player.SendActionFailed();
        }
    }
}