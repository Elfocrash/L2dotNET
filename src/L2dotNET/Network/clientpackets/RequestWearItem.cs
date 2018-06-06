using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestWearItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _unknow;
        private readonly int _listId;
        private readonly int _count;
        private readonly int[] _items;

        public RequestWearItem(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
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

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                for (int i = 0; i < _count; i++)
                {
                    int itemId = _items[i];

                    player.SendMessageAsync($"wear item {itemId}");
                }
            });
        }
    }
}