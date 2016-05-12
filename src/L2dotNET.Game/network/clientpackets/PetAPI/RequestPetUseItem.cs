using L2dotNET.GameService.model.items;
using L2dotNET.GameService.model.playable;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestPetUseItem : GameServerNetworkRequest
    {
        private int sID;
        public RequestPetUseItem(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            sID = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Summon == null || !(player.Summon is L2Pet))
            {
                player.sendActionFailed();
                return;
            }

            L2Pet pet = (L2Pet)player.Summon;

            if (pet._p_block_act == 1 || pet.Dead)
            {
                player.sendActionFailed();
                return;
            }

            if (!pet.Inventory.Items.ContainsKey(sID))
            {
                player.sendSystemMessage(352); //Incorrect item.
                player.sendActionFailed();
                return;
            }

            L2Item item = pet.Inventory.Items[sID];

            if (ItemHandler.Instance.Process(pet, item))
                return;

            player.sendActionFailed();
        }
    }
}
