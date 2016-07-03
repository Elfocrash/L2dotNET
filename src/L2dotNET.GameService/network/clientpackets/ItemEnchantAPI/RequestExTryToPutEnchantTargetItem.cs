using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.ItemEnchantAPI
{
    class RequestExTryToPutEnchantTargetItem : GameServerNetworkRequest
    {
        private int a_sTargetID;

        public RequestExTryToPutEnchantTargetItem(GameClient client, byte[] data)
        {
            makeme(client, data, 2);
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

            L2Item item = player.GetItemByObjId(a_sTargetID);

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