using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class Appearing : GameServerNetworkRequest
    {
        public Appearing(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            // nothing
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            int x = player.X;
            int y = player.Y;

            if (player._obsx != -1)
            {
                x = player._obsx;
                y = player._obsy;
            }

            player.SendPacket(new UserInfo(player));
            player.ValidateVisibleObjects(x, y, false);
            player.UpdateVisibleStatus();

            if (player.Summon != null)
            {
                player.Summon.ValidateVisibleObjects(x, y, false);
                player.Summon.isTeleporting = false;
            }

            player.SendActionFailed();
        }
    }
}