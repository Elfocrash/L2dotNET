using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestShowMiniMap : GameServerNetworkRequest
    {
        public RequestShowMiniMap(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            // do nothing
        }

        public override void run()
        {
            Client.sendPacket(new ShowMiniMap());
        }
    }
}