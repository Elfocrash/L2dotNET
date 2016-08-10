using L2dotNET.managers;
using L2dotNET.model.items;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.ItemEnchantAPI
{
    class RequestExTryToPutEnchantTargetItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _aSTargetId;

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