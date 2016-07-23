using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestSaveInventoryOrder : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _count;
        private readonly int[] _items;

        public RequestSaveInventoryOrder(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
            _count = packet.ReadInt();

            //  _count = Math.Min(125, _count); мм?зачем
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

            foreach (L2Item item in player.Inventory.Items)
            {
                for (int i = 0; i < _count; i++)
                {
                    int objId = _items[i * 2];
                    int loc = _items[(i * 2) + 1];

                    if (item.ObjId == objId)
                        item.SlotLocation = loc;
                }
            }
        }
    }
}