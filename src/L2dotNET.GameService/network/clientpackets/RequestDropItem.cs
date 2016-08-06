using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
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