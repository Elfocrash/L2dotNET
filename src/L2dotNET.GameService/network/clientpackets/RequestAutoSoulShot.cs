using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAutoSoulShot : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _itemId;
        private readonly int _type;

        public RequestAutoSoulShot(Packet packet, GameClient client)
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