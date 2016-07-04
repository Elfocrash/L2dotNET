using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.ItemEnchantAPI
{
    class RequestExCancelEnchantItem : GameServerNetworkRequest
    {
        public RequestExCancelEnchantItem(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            // nothing
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            player.EnchantScroll = null;

            switch (player.EnchantState)
            {
                case ItemEnchantManager.StateEnchantStart:
                    player.EnchantItem = null;
                    break;
            }

            player.EnchantState = 0;
            player.SendPacket(new EnchantResult(EnchantResultVal.CloseWindow));
        }
    }
}