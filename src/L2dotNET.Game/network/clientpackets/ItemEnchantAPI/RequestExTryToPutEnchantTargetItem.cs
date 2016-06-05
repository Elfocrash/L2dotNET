using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.items;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets.ItemEnchantAPI
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
                player.sendSystemMessage(SystemMessage.SystemMessageId.INAPPROPRIATE_ENCHANT_CONDITION);
                player.sendActionFailed();
                return;
            }

            L2Item item = player.Inventory.getByObject(a_sTargetID);

            if (item == null)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.INAPPROPRIATE_ENCHANT_CONDITION);
                player.sendActionFailed();
                return;
            }

            ItemEnchantManager.getInstance().tryPutItem(player, item);
        }
    }
}