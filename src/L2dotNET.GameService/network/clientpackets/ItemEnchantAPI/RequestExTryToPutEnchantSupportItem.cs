using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.ItemEnchantAPI
{
    class RequestExTryToPutEnchantSupportItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _aSSupportId;
        private readonly int _aSTargetId;

        public RequestExTryToPutEnchantSupportItem(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
            _aSSupportId = packet.ReadInt();
            _aSTargetId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if ((player.EnchantState != ItemEnchantManager.StateEnchantStart) || (player.EnchantItem.ObjId != _aSTargetId))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.RegistrationOfEnhancementSpellbookHasFailed);
                player.SendActionFailed();
                return;
            }

            L2Item stone = player.GetItemByObjId(_aSSupportId);

            if (stone == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.RegistrationOfEnhancementSpellbookHasFailed);
                player.SendActionFailed();
                return;
            }

            ItemEnchantManager.GetInstance().TryPutStone(player, stone);
        }
    }
}