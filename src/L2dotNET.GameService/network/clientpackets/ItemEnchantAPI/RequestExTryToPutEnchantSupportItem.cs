using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.ItemEnchantAPI
{
    class RequestExTryToPutEnchantSupportItem : GameServerNetworkRequest
    {
        private int _aSSupportId;
        private int _aSTargetId;

        public RequestExTryToPutEnchantSupportItem(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            _aSSupportId = ReadD();
            _aSTargetId = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

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