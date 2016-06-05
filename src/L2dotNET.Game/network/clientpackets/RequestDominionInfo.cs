using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets
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