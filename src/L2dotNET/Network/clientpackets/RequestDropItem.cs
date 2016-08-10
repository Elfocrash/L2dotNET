using L2dotNET.model.player;

namespace L2dotNET.Network.clientpackets
{
    class RequestDropItem : PacketBase
    {
        private readonly GameClient _client;
        private  int objectId;
        private int count;
        private int x;
        private int y;
        private int z;

        public RequestDropItem(Packet packet, GameClient client)
        {
            _client = client;
            objectId = packet.ReadInt();
            count = packet.ReadInt();
            x = packet.ReadInt();
            y = packet.ReadInt();
            z = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.DropItem(objectId, count, x, y, z);
        }
    }
}