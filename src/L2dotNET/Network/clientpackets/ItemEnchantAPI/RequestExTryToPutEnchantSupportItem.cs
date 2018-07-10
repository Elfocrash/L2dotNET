using System;
using System.Threading.Tasks;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Managers;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.ItemEnchantAPI
{
    class RequestExTryToPutEnchantSupportItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _aSSupportId;
        private readonly int _aSTargetId;

        public RequestExTryToPutEnchantSupportItem(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
            _aSSupportId = packet.ReadInt();
            _aSTargetId = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                if ((player.EnchantState != ItemEnchantManager.StateEnchantStart) ||
                    (player.EnchantItem.ObjectId != _aSTargetId))
                {
                    player.SendSystemMessage(SystemMessageId.RegistrationOfEnhancementSpellbookHasFailed);
                    player.SendActionFailedAsync();
                    return;
                }

                L2Item stone = player.GetItemByObjId(_aSSupportId);

                if (stone == null)
                {
                    player.SendSystemMessage(SystemMessageId.RegistrationOfEnhancementSpellbookHasFailed);
                    player.SendActionFailedAsync();
                    return;
                }

                ItemEnchantManager.GetInstance().TryPutStone(player, stone);
            });
        }
    }
}