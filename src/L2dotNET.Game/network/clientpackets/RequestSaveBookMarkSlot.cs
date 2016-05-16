
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
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
                player.sendSystemMessage(SystemMessage.SystemMessageId.NO_SPACE_TO_SAVE_TELEPORT_LOCATION);
                player.sendActionFailed();
                return;
            }

            player.Telbook.SaveMark(player, name, icon, tag);
        }
    }
}
