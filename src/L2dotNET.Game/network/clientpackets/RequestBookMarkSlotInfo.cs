using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestBookMarkSlotInfo : GameServerNetworkRequest
    {
        public RequestBookMarkSlotInfo(GameClient client, byte[] data)
        {
            base.makeme(client, data, 6);
        }

        public override void read()
        {
            // nothing
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            player.sendPacket(new ExGetBookMarkInfo(player.TelbookLimit, player.Telbook));
        }
    }
}