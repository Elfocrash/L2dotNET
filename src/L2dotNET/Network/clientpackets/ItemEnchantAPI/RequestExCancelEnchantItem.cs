using L2dotNET.managers;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.ItemEnchantAPI
{
    class RequestExCancelEnchantItem : PacketBase
    {
        private readonly GameClient _client;

        public RequestExCancelEnchantItem(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.EnchantScroll = null;

            switch (player.EnchantState)
            {
                case ItemEnchantManager.StateEnchantStart:
                    player.EnchantItem = null;
                    break;
            }

            player.EnchantState = 0;
            player.SendPacket(new EnchantResult(EnchantResultVal.CloseWindow));
        }
    }
}