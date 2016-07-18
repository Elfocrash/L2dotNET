using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.PetAPI
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