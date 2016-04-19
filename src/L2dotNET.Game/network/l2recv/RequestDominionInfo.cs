using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.network.l2recv
{
    class RequestDominionInfo : GameServerNetworkRequest
    {
        public RequestDominionInfo(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            //nothing
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            player.sendPacket(new ExReplyDominionInfo());
            player.sendPacket(new ExShowOwnthingPos());
        }
    }
}
