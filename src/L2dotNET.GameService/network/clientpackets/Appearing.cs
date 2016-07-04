using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class Appearing : GameServerNetworkRequest
    {
        public Appearing(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            // nothing
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

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