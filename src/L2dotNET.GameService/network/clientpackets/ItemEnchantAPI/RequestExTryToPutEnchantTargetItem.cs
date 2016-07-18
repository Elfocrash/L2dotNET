using L2dotNET.GameService.Config;
using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.ItemEnchantAPI
{
    class RequestExTryToPutEnchantTargetItem : PacketBase
    {
        private int _aSTargetId;
        private GameClient _client;
        public RequestExTryToPutEnchantTargetItem(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
            _aSTargetId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

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