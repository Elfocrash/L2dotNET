
namespace L2dotNET.Game.network.l2recv
{
    class RequestSaveBookMarkSlot : GameServerNetworkRequest
    {
        private string name;
        private int icon;
        private string tag;
        public RequestSaveBookMarkSlot(GameClient client, byte[] data)
        {
            base.makeme(client, data, 6);
        }

        public override void read()
        {
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

            player.Telbook.SaveMark(player, name, icon, tag);
        }
    }
}
