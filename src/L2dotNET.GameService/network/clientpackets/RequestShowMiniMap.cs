using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestShowMiniMap : GameServerNetworkRequest
    {
        public RequestShowMiniMap(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            // do nothing
        }

        public override void Run()
        {
            Client.SendPacket(new ShowMiniMap());
        }
    }
}