using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAutoSoulShot : PacketBase
    {
        private int _itemId;
        private int _type;
        private readonly GameClient _client;

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