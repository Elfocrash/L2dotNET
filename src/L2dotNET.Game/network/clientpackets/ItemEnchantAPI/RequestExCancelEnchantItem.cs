using L2dotNET.GameService.managers;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestExCancelEnchantItem : GameServerNetworkRequest
    {
        public RequestExCancelEnchantItem(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            // nothing
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            player.EnchantScroll = null;

            switch (player.EnchantState)
            {
                case ItemEnchantManager.STATE_ENCHANT_START:
                    player.EnchantItem = null;
                    break;
            }

            player.EnchantState = 0;
            player.sendPacket(new EnchantResult(EnchantResultVal.closeWindow));
        }
    }
}