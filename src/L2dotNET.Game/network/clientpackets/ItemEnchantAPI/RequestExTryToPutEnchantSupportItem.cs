using L2dotNET.GameService.managers;
using L2dotNET.GameService.model.items;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestExTryToPutEnchantSupportItem : GameServerNetworkRequest
    {
        private int a_sSupportID;
        private int a_sTargetID;
        public RequestExTryToPutEnchantSupportItem(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            a_sSupportID = readD();
            a_sTargetID = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.EnchantState != ItemEnchantManager.STATE_ENCHANT_START || player.EnchantItem.ObjID != a_sTargetID)
            {
                player.sendSystemMessage(2387);//Registration of the support enhancement spellbook has failed.
                player.sendActionFailed();
                return;
            }

            L2Item stone = player.Inventory.getByObject(a_sSupportID);

            if (stone == null)
            {
                player.sendSystemMessage(2387);//Registration of the support enhancement spellbook has failed.
                player.sendActionFailed();
                return;
            }

            ItemEnchantManager.getInstance().tryPutStone(player, stone);
        }
    }
}
