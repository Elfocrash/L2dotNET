using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestDestroyItem : GameServerNetworkRequest
    {
        public RequestDestroyItem(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        private int sID;
        private int num;

        public override void read()
        {
            sID = readD();
            num = readD();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (player._p_block_act == 1)
            {
                player.sendActionFailed();
                return;
            }

            if (player.TradeState != 0)
            {
                player.sendMessage("You cannot destroy items while trading.");
                player.sendActionFailed();
                return;
            }

            L2Item item = player.GetItemByObjId(sID);

            if (item == null)
            {
                player.sendMessage("null item " + sID);
                player.sendActionFailed();
                return;
            }

            if ((item.Template.can_equip_hero == 1) && (item.Template.Type == ItemTemplate.L2ItemType.weapon))
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.HERO_WEAPONS_CANT_DESTROYED);
                player.sendActionFailed();
                return;
            }

            if (item.Template.is_destruct == 0)
            {
                SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1_S2);
                sm.AddItemName(item.Template.ItemID);
                sm.AddString("cannot be destroyed.");
                player.sendPacket(sm);
                player.sendActionFailed();
                return;
            }

            if (num < 0)
                num = 1;

            //if (item._isEquipped == 1)
            //{
            //    int pdollId = player.Inventory.getPaperdollId(item.Template);
            //    player.setPaperdoll(pdollId, null, true);
            //    player.broadcastUserInfo();
            //}

            player.DestroyItemById(item.Template.ItemID, num);
        }
    }
}