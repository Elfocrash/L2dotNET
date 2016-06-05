using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.network.clientpackets
{
    class RequestWearItem : GameServerNetworkRequest
    {
        private int _unknow;
        private int _listId;
        private int _count;
        private int[] _items;

        public RequestWearItem(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            _unknow = readD();
            _listId = readD(); // List of ItemID to Wear
            _count = readD(); // Number of Item to Wear

            if (_count < 0)
                _count = 0;
            if (_count > 100)
                _count = 0; // prevent too long lists

            // Create _items table that will contain all ItemID to Wear
            _items = new int[_count];

            // Fill _items table with all ItemID to Wear
            for (int i = 0; i < _count; i++)
            {
                _items[i] = readD();
            }
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            for (int i = 0; i < _count; i++)
            {
                int itemId = _items[i];

                player.sendMessage("wear item " + itemId);
            }
        }
    }
}