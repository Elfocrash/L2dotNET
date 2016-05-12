using L2dotNET.GameService.model.items;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestSaveInventoryOrder : GameServerNetworkRequest
    {
        private int _count;
        private int[] _items;
        public RequestSaveInventoryOrder(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            _count = readD();

          //  _count = Math.Min(125, _count); мм?зачем
            _items = new int[_count * 2];
            for (int i = 0; i < _count; i++)
            {
                _items[i * 2] = readD();
                _items[i * 2 + 1] = readD();
            }
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            foreach (L2Item item in player.Inventory.Items.Values)
            {
                for (int i = 0; i < _count; i++)
                {
                    int objId = _items[i * 2];
                    int loc = _items[i * 2 + 1];

                    if (item.ObjID == objId)
                    {
                        item.SlotLocation = loc;
                    }
                }
            }
        }
    }
}
