using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestUnEquipItem : GameServerNetworkRequest
    {
        public RequestUnEquipItem(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int slotBitType;

        public override void read()
        {
            slotBitType = readD();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (player._p_block_act == 1)
            {
                player.sendActionFailed();
                return;
            }

            int dollId = player.Inventory.getPaperdollIdByMask(slotBitType);

            player.setPaperdoll(dollId, null, true);
            player.broadcastUserInfo();
        }
    }
}