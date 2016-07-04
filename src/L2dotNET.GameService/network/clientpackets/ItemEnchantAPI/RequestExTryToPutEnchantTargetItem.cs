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
                player.SendSystemMessage(SystemMessage.SystemMessageId.INAPPROPRIATE_ENCHANT_CONDITION);
                player.SendActionFailed();
                return;
            }

            L2Item item = player.GetItemByObjId(a_sTargetID);

            if (item == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.INAPPROPRIATE_ENCHANT_CONDITION);
                player.SendActionFailed();
                return;
            }

            ItemEnchantManager.getInstance().tryPutItem(player, item);
        }
    }
}