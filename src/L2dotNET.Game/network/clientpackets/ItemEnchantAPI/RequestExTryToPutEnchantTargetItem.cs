using L2dotNET.GameService.managers;
using L2dotNET.GameService.model.items;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestExTryToPutEnchantTargetItem : GameServerNetworkRequest
    {
        private int a_sTargetID;
        public RequestExTryToPutEnchantTargetItem(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            a_sTargetID = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.EnchantState != ItemEnchantManager.STATE_PUT_ITEM)
            {
                player.sendSystemMessage(355);//Inappropriate enchant conditions.
                player.sendActionFailed();
                return;
            }

            L2Item item = player.Inventory.getByObject(a_sTargetID);

            if (item == null)
            {
                player.sendSystemMessage(355);//Inappropriate enchant conditions.
                player.sendActionFailed();
                return;
            }

            ItemEnchantManager.getInstance().tryPutItem(player, item);
        }
    }
}
