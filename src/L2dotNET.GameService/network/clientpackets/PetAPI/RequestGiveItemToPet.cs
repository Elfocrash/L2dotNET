using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.PetAPI
{
    class RequestGiveItemToPet : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _sId;
        private int _num;

        public RequestGiveItemToPet(Packet packet, GameClient client)
        {
            _client = client;
            _sId = packet.ReadInt();
            _num = packet.ReadInt();
            if (_num < 0)
                _num = 1;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (!(player.Summon is L2Pet) || (player.EnchantState != 0))
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

            if (!item.Template.Dropable || !item.Template.Destroyable || !item.Template.Tradable || !item.Template.HeroItem || (pet.ControlItem.ObjId == _sId))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ItemNotForPets);
                player.SendActionFailed();
                return;
            }

            if (_num > item.Count)
                _num = item.Count;

            //List<long[]> items = new List<long[]>
            //                     {
            //                         new[] { _sId, _num }
            //                     };
            //pet.Inventory.transferHere(player, items, true);
        }
    }
}