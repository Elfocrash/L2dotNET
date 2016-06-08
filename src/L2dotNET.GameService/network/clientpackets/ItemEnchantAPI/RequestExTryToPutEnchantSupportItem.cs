using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.ItemEnchantAPI
{
    class RequestExTryToPutEnchantSupportItem : GameServerNetworkRequest
    {
        private int a_sSupportID;
        private int a_sTargetID;

        public RequestExTryToPutEnchantSupportItem(GameClient client, byte[] data)
        {
            makeme(client, data, 2);
        }

        public override void read()
        {
            a_sSupportID = readD();
            a_sTargetID = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if ((player.EnchantState != ItemEnchantManager.STATE_ENCHANT_START) || (player.EnchantItem.ObjID != a_sTargetID))
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.REGISTRATION_OF_ENHANCEMENT_SPELLBOOK_HAS_FAILED);
                player.sendActionFailed();
                return;
            }

            L2Item stone = player.Inventory.getByObject(a_sSupportID);

            if (stone == null)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.REGISTRATION_OF_ENHANCEMENT_SPELLBOOK_HAS_FAILED);
                player.sendActionFailed();
                return;
            }

            ItemEnchantManager.getInstance().tryPutStone(player, stone);
        }
    }
}