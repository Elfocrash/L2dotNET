using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets
{
    class RequestShowMiniMap : GameServerNetworkRequest
    {
        public RequestShowMiniMap(GameClient client, byte[] data)
        {
            base.makeme(client, data);
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