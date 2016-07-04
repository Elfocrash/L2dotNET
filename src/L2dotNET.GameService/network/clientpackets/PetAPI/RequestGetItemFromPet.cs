using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.PetAPI
{
    class RequestGetItemFromPet : GameServerNetworkRequest
    {
        private int _objectId;
        private long _count;
        private int _equipped;

        public RequestGetItemFromPet(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _objectId = ReadD();
            _count = ReadQ();
            _equipped = ReadD();
        }

        public override void Run()
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
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotGiveItemsToDeadPet);
                player.SendActionFailed();
                return;
            }

            //if (!pet.Inventory.Items.Contains(objectId))
            //{
            //    player.sendActionFailed();
            //    return;
            //}

            L2Item item = pet.Inventory.Items[_objectId];

            if (item.TempBlock)
            {
                player.SendActionFailed();
                return;
            }

            if ((item.Template.IsDrop == 0) || (item.Template.IsDestruct == 0) || (item.Template.IsTrade == 0) || (item.Template.CanEquipHero != -1) || (pet.ControlItem.ObjId == _objectId))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ItemNotForPets);
                player.SendActionFailed();
                return;
            }

            if (_count < 0)
                _count = 1;
            else if (_count > item.Count)
                _count = item.Count;

            List<long[]> items = new List<long[]>();
            items.Add(new[] { _objectId, _count });
            //pet.Inventory.transferFrom(player, items, true);
        }
    }
}