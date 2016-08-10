using L2dotNET.model.items;
using L2dotNET.model.playable;
using L2dotNET.model.player;

namespace L2dotNET.Network.clientpackets.PetAPI
{
    class RequestPetUseItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _sId;

        public RequestPetUseItem(Packet packet, GameClient client)
        {
            _client = client;
            _sId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (!(player.Summon is L2Pet))
            {
                player.SendActionFailed();
                return;
            }

            L2Pet pet = (L2Pet)player.Summon;

            if ((pet.PBlockAct == 1) || pet.Dead)
            {
                player.SendActionFailed();
                return;
            }

            //if (!pet.Inventory.Items.ContainsKey(sID))
            //{
            //    player.sendSystemMessage(SystemMessage.SystemMessageId.INCORRECT_ITEM);
            //    player.sendActionFailed();
            //    return;
            //}

            L2Item item = pet.Inventory.Items[_sId];

            if (ItemHandler.Instance.Process(pet, item))
                return;

            player.SendActionFailed();
        }
    }
}