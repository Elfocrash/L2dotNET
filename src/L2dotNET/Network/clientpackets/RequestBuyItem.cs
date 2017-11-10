using System.Linq;
using L2dotNET.model.npcs;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.tables;

namespace L2dotNET.Network.clientpackets
{
    class RequestBuyItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _listId;
        private readonly int _count;
        private readonly int[] _items;

        public RequestBuyItem(Packet packet, GameClient client)
        {
            _client = client;
            _listId = packet.ReadInt();
            _count = packet.ReadInt();

            _items = new int[_count * 2];
            for (int i = 0; i < _count; i++)
            {
                _items[i * 2] = packet.ReadInt();
                _items[(i * 2) + 1] = packet.ReadInt();
            }
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
        }
    }
}