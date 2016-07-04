using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.PetAPI
{
    class RequestGiveItemToPet : GameServerNetworkRequest
    {
        private int sID;
        private long Num;

        public RequestGiveItemToPet(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            sID = readD();
            Num = readQ();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if ((player.Summon == null) || !(player.Summon is L2Pet) || (player.EnchantState != 0))
            {
                player.SendActionFailed();
                return;
            }

            L2Pet pet = (L2Pet)player.Summon;

            if (pet.Dead)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CANNOT_GIVE_ITEMS_TO_DEAD_PET);
                player.SendActionFailed();
                return;
            }

            L2Item item = player.GetItemByObjId(sID);

            if ((item == null) || item.TempBlock)
            {
                player.SendActionFailed();
                return;
            }

            if ((item.Template.is_drop == 0) || (item.Template.is_destruct == 0) || (item.Template.is_trade == 0) || (item.Template.can_equip_hero != -1) || (pet.ControlItem.ObjId == sID))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ITEM_NOT_FOR_PETS);
                player.SendActionFailed();
                return;
            }

            if (Num < 0)
                Num = 1;
            else if (Num > item.Count)
                Num = item.Count;

            List<long[]> items = new List<long[]>();
            items.Add(new[] { sID, Num });
            //pet.Inventory.transferHere(player, items, true);
        }
    }
}