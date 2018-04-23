using System;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestAutoSoulShot : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _itemId;
        private readonly int _type;

        public RequestAutoSoulShot(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
            _itemId = packet.ReadInt();
            _type = packet.ReadInt(); //1 - enable
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            L2Item item = player.Inventory.GetItemByItemId(_itemId);
        }
    }
}