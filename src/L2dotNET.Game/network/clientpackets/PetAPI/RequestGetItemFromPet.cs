using System.Collections.Generic;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.model.playable;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestGetItemFromPet : GameServerNetworkRequest
    {
        private int objectId;
        private long count;
        private int equipped;
        public RequestGetItemFromPet(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            objectId = readD();
            count = readQ();
            equipped = readD();
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

            if (pet.Dead)
            {
                player.sendSystemMessage(590);//Your pet is dead and any attempt you make to give it something goes unrecognized.
                player.sendActionFailed();
                return;
            }

            if (!pet.Inventory.Items.ContainsKey(objectId))
            {
                player.sendActionFailed();
                return;
            }

            L2Item item = pet.Inventory.Items[objectId];

            if (item.TempBlock)
            {
                player.sendActionFailed();
                return;
            }

            if (item.Template.is_drop == 0 || item.Template.is_destruct == 0 || item.Template.is_trade == 0 || item.Template.can_equip_hero != -1 || pet.ControlItem.ObjID == objectId)
            {
                player.sendSystemMessage(544);//Your pet cannot carry this item.
                player.sendActionFailed();
                return;
            }

            if (count < 0)
                count = 1;
            else if (count > item.Count)
                count = item.Count;

            List<long[]> items = new List<long[]>();
            items.Add(new long[] {objectId, count});
            pet.Inventory.transferFrom(player, items, true);
        }
    }
}
