using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestTeleportBookMark : GameServerNetworkRequest
    {
        private int id;

        public RequestTeleportBookMark(GameClient client, byte[] data)
        {
            base.makeme(client, data, 6);
        }

        public override void read()
        {
            id = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Telbook == null)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.NO_SPACE_TO_SAVE_TELEPORT_LOCATION);
                player.sendActionFailed();
                return;
            }

            player.Telbook.UseMark(player, (byte)id);
        }
    }
}