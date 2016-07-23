using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestWearItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _unknow;
        private readonly int _listId;
        private readonly int _count;
        private readonly int[] _items;

        public RequestWearItem(Packet packet, GameClient client)
        {
            _client = client;
            _unknow = packet.ReadInt();
            _listId = packet.ReadInt(); // List of ItemID to Wear
            _count = packet.ReadInt(); // Number of Item to Wear

            if (_count < 0)
                _count = 0;
            if (_count > 100)
                _count = 0; // prevent too long lists

            // Create _items table that will contain all ItemID to Wear
            _items = new int[_count];

            // Fill _items table with all ItemID to Wear
            for (int i = 0; i < _count; i++)
                _items[i] = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            for (int i = 0; i < _count; i++)
            {
                int itemId = _items[i];

                player.SendMessage($"wear item {itemId}");
            }
        }
    }
}