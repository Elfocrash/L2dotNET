using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.ItemEnchantAPI
{
    class RequestExTryToPutEnchantTargetItem : GameServerNetworkRequest
    {
        private int _aSTargetId;

        public RequestExTryToPutEnchantTargetItem(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            _aSTargetId = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.EnchantState != ItemEnchantManager.StatePutItem)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.InappropriateEnchantCondition);
                player.SendActionFailed();
                return;
            }

            L2Item item = player.GetItemByObjId(_aSTargetId);

            if (item == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.InappropriateEnchantCondition);
                player.SendActionFailed();
                return;
            }

            ItemEnchantManager.GetInstance().TryPutItem(player, item);
        }
    }
}