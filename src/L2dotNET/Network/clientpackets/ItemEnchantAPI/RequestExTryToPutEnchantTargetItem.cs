using System;
using System.Threading.Tasks;
using L2dotNET.Managers;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.ItemEnchantAPI
{
    class RequestExTryToPutEnchantTargetItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _aSTargetId;

        public RequestExTryToPutEnchantTargetItem(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
            _aSTargetId = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                if (player.EnchantState != ItemEnchantManager.StatePutItem)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.InappropriateEnchantCondition);
                    player.SendActionFailedAsync();
                    return;
                }

                L2Item item = player.GetItemByObjId(_aSTargetId);

                if (item == null)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.InappropriateEnchantCondition);
                    player.SendActionFailedAsync();
                    return;
                }

                ItemEnchantManager.GetInstance().TryPutItem(player, item);
            });
        }
    }
}