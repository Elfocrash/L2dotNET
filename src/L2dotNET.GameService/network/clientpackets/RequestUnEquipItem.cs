using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestUnEquipItem : GameServerNetworkRequest
    {
        public RequestUnEquipItem(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _slotBitType;

        public override void Read()
        {
            _slotBitType = ReadD();
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            //int dollId = player.Inventory.getPaperdollIdByMask(slotBitType);

            //player.setPaperdoll(dollId, null, true);
            player.BroadcastUserInfo();
        }
    }
}