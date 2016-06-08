using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestDominionInfo : GameServerNetworkRequest
    {
        public RequestDominionInfo(GameClient client, byte[] data)
        {
            makeme(client, data, 2);
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