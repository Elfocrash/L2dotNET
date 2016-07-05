using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestWearItem : GameServerNetworkRequest
    {
        private int _unknow;
        private int _listId;
        private int _count;
        private int[] _items;

        public RequestWearItem(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _unknow = ReadD();
            _listId = ReadD(); // List of ItemID to Wear
            _count = ReadD(); // Number of Item to Wear

            if (_count < 0)
            {
                _count = 0;
            }
            if (_count > 100)
            {
                _count = 0; // prevent too long lists
            }

            // Create _items table that will contain all ItemID to Wear
            _items = new int[_count];

            // Fill _items table with all ItemID to Wear
            for (int i = 0; i < _count; i++)
            {
                _items[i] = ReadD();
            }
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            for (int i = 0; i < _count; i++)
            {
                int itemId = _items[i];

                player.SendMessage("wear item " + itemId);
            }
        }
    }
}