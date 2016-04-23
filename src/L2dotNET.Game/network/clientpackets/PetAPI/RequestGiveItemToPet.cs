using System.Collections.Generic;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.playable;

namespace L2dotNET.Game.network.l2recv
{
    class RequestGiveItemToPet : GameServerNetworkRequest
    {
        private int sID;
        private long Num;
        public RequestGiveItemToPet(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            sID = readD();
            Num = readQ();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Summon == null || !(player.Summon is L2Pet) || player.EnchantState != 0)
            {
                player.sendActionFailed();
                return;
            }

            L2Pet pet = (L2Pet)player.Summon;

            if (pet._isDead)
            {
                player.sendSystemMessage(590);//Your pet is dead and any attempt you make to give it something goes unrecognized.
                player.sendActionFailed();
                return;
            }

            L2Item item = player.Inventory.getByObject(sID);

            if (item == null || item.TempBlock)
            {
                player.sendActionFailed();
                return;
            }

            if (item.Template.is_drop == 0 || item.Template.is_destruct == 0 || item.Template.is_trade == 0 || item.Template.can_equip_hero != -1 || pet.ControlItem.ObjID == sID)
            {
                player.sendSystemMessage(544);//Your pet cannot carry this item.
                player.sendActionFailed();
                return;
            }

            if (Num < 0)
                Num = 1;
            else if (Num > item.Count)
                Num = item.Count;

            List<long[]> items = new List<long[]>();
            items.Add(new long[] {sID, Num});
            pet.Inventory.transferHere(player, items, true);
        }
    }
}
