using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestBuySellUiClose : PacketBase
    {
        private readonly GameClient _client;

        public RequestBuySellUiClose(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.SendItemList(true);
        }
    }
}