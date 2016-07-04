using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestDominionInfo : GameServerNetworkRequest
    {
        public RequestDominionInfo(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            //nothing
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            player.SendPacket(new ExReplyDominionInfo());
            player.SendPacket(new ExShowOwnthingPos());
        }
    }
}