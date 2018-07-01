using System;
using System.Threading.Tasks;
using L2dotNET.Managers;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.ItemEnchantAPI
{
    class RequestExCancelEnchantItem : PacketBase
    {
        private readonly GameClient _client;

        public RequestExCancelEnchantItem(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                var player = _client.CurrentPlayer;

                player.EnchantScroll = null;

                switch (player.EnchantState)
                {
                    case ItemEnchantManager.StateEnchantStart:
                        player.EnchantItem = null;
                        break;
                }

                player.EnchantState = 0;
                player.SendPacketAsync(new EnchantResult(EnchantResultVal.CloseWindow));
            });
        }
    }
}