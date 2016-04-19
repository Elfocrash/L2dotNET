using L2dotNET.Game.managers;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.model.items.effects
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
                player.sendSystemMessage(2188);//Another enchantment is in progress. Please complete the previous task, then try again
                player.sendActionFailed();
                return;
            }

            player.sendPacket(new ChooseInventoryItem(item.Template.ItemID));
            player.EnchantScroll = item;
            player.EnchantState = ItemEnchantManager.STATE_PUT_ITEM;
            player.sendSystemMessage(303);//Select item to enchant.
        }
    }
}
