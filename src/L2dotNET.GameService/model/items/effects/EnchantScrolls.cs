using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Items.Effects
{
    class EnchantScrolls : ItemEffect
    {
        public EnchantScrolls()
        {
            Ids = ItemEnchantManager.GetInstance().GetIds();
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            if (player.EnchantState != 0)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.AnotherEnchantmentIsInProgress);
                player.SendActionFailed();
                return;
            }

            player.SendPacket(new ChooseInventoryItem(item.Template.ItemId));
            player.EnchantScroll = item;
            player.EnchantState = ItemEnchantManager.StatePutItem;
            player.SendSystemMessage(SystemMessage.SystemMessageId.SelectItemToEnchant);
        }
    }
}