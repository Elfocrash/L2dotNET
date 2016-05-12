using L2dotNET.GameService.model.items;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.model.skills2.effects
{
    class a_convert_item : TEffect
    {
        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            L2Player player = caster as L2Player;
            L2Item item = player.getWeaponItem();
            if (item == null || !ItemTable.getInstance().ConvertDataList.ContainsKey(item.Template.ItemID))
            {
                caster.sendSystemMessage(2130);//You cannot convert this item.
                return nothing;
            }

            int newid = ItemTable.getInstance().ConvertDataList[item.Template.ItemID];

            int pdollId = player.Inventory.getPaperdollId(item.Template);
            player.setPaperdoll(pdollId, null, false);
            player.broadcastUserInfo();

            int oldweight = item.Template.Weight;
            item.Template = ItemTable.getInstance().getItem(newid);
            item.sql_update();

            if (oldweight != item.Template.Weight)
                player.updateWeight();

            player.setPaperdoll(pdollId, item, false);
            player.broadcastUserInfo();

            InventoryUpdate iu = new InventoryUpdate();
            iu.addModItem(item);
            player.sendPacket(iu);

            return nothing;
        }

        public override bool canUse(world.L2Character caster)
        {
            L2Item item = caster.getWeaponItem();
            if (item == null)
            {
                caster.sendSystemMessage(2130);//You cannot convert this item.
                return false;
            }

            if (item.AugmentationID > 0)
            {
                caster.sendSystemMessage(2129);//The augmented item cannot be converted. Please convert after the augmentation has been removed.
                return false;
            }

            if (!ItemTable.getInstance().ConvertDataList.ContainsKey(item.Template.ItemID))
            {
                caster.sendSystemMessage(2130);//You cannot convert this item.
                return false;
            }

            return true;
        }

    }
}
