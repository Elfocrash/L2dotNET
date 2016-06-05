using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Items.Effects
{
    class EnchantScrolls : ItemEffect
    {
        public EnchantScrolls()
        {
            ids = ItemEnchantManager.getInstance().getIds();
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            if (player.EnchantState != 0)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.ANOTHER_ENCHANTMENT_IS_IN_PROGRESS);
                player.sendActionFailed();
                return;
            }

            player.sendPacket(new ChooseInventoryItem(item.Template.ItemID));
            player.EnchantScroll = item;
            player.EnchantState = ItemEnchantManager.STATE_PUT_ITEM;
            player.sendSystemMessage(SystemMessage.SystemMessageId.SELECT_ITEM_TO_ENCHANT);
        }
    }
}