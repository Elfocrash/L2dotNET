using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.PetAPI
{
    class RequestGiveItemToPet : GameServerNetworkRequest
    {
        private int _sId;
        private long _num;

        public RequestGiveItemToPet(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _sId = ReadD();
            _num = ReadQ();
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

            L2Item item = player.GetItemByObjId(_sId);

            if ((item == null) || item.TempBlock)
            {
                player.SendActionFailed();
                return;
            }

            if ((item.Template.IsDrop == 0) || (item.Template.IsDestruct == 0) || (item.Template.IsTrade == 0) || (item.Template.CanEquipHero != -1) || (pet.ControlItem.ObjId == _sId))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ItemNotForPets);
                player.SendActionFailed();
                return;
            }

            if (_num < 0)
                _num = 1;
            else if (_num > item.Count)
                _num = item.Count;

            List<long[]> items = new List<long[]>();
            items.Add(new[] { _sId, _num });
            //pet.Inventory.transferHere(player, items, true);
        }
    }
}