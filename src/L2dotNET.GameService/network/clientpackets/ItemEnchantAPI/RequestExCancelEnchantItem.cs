using L2dotNET.GameService.Config;
using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.ItemEnchantAPI
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