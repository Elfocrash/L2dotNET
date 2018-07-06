using System;
using System.Threading.Tasks;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestSaveInventoryOrder : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _count;
        private readonly int[] _items;

        public RequestSaveInventoryOrder(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
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

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                foreach (L2Item item in player.Inventory.Items)
                {
                    for (int i = 0; i < _count; i++)
                    {
                        int objId = _items[i * 2];
                        int loc = _items[(i * 2) + 1];

                        if (item.ObjectId == objId)
                            item.SlotLocation = loc;
                    }
                }
            });
        }
    }
}