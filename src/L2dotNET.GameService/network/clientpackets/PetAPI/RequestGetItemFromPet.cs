using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.PetAPI
{
    class RequestGetItemFromPet : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _objectId;
        private int _count;
        private readonly int _equipped;

        public RequestGetItemFromPet(Packet packet, GameClient client)
        {
            _client = client;
            _objectId = packet.ReadInt();
            _count = packet.ReadInt();
            if (_count < 0)
                _count = 1;
            _equipped = packet.ReadInt();
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

            if (!item.Template.Dropable || !item.Template.Destroyable || !item.Template.Tradable || !item.Template.HeroItem || (pet.ControlItem.ObjId == _objectId))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ItemNotForPets);
                player.SendActionFailed();
                return;
            }

            if (_count > item.Count)
                _count = item.Count;

            //List<long[]> items = new List<long[]>
            //                     {
            //                         new[] { _objectId, _count }
            //                     };
            //pet.Inventory.transferFrom(player, items, true);
        }
    }
}