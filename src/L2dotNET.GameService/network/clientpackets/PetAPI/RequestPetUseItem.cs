using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.PetAPI
{
    class RequestPetUseItem : GameServerNetworkRequest
    {
        private int _sId;

        public RequestPetUseItem(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _sId = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            if ((player.Summon == null) || !(player.Summon is L2Pet))
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