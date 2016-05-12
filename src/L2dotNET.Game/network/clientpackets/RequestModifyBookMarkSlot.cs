
namespace L2dotNET.GameService.network.l2recv
{
    class RequestModifyBookMarkSlot : GameServerNetworkRequest
    {
        private string name;
        private int icon;
        private string tag;
        private int id;
        public RequestModifyBookMarkSlot(GameClient client, byte[] data)
        {
            base.makeme(client, data, 6);
        }

        public override void read()
        {
            id = readD();
            name = readS();
            icon = readD();
            tag = readS();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (player.Telbook == null)
            {
                player.sendSystemMessage(2358); //You have no space to save the teleport location.
                player.sendActionFailed();
                return;
            }

            player.Telbook.ModifyMark(player, (byte)id, name, icon, tag);
        }
    }
}
