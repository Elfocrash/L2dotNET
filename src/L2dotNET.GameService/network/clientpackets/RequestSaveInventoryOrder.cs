using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestSaveInventoryOrder : GameServerNetworkRequest
    {
        private int _count;
        private int[] _items;

        public RequestSaveInventoryOrder(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            _count = ReadD();

            //  _count = Math.Min(125, _count); мм?зачем
            _items = new int[_count * 2];
            for (int i = 0; i < _count; i++)
            {
                _items[i * 2] = ReadD();
                _items[(i * 2) + 1] = ReadD();
            }
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            foreach (L2Item item in player.Inventory.Items)
                for (int i = 0; i < _count; i++)
                {
                    int objId = _items[i * 2];
                    int loc = _items[(i * 2) + 1];

                    if (item.ObjId == objId)
                    {
                        item.SlotLocation = loc;
                    }
                }
        }
    }
}