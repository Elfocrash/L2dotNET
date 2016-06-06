using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class a_convert_item : TEffect
    {
        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            L2Player player = caster as L2Player;
            if (player != null)
            {
                L2Item item = player.getWeaponItem();
                if (item == null || !ItemTable.Instance.ConvertDataList.ContainsKey(item.Template.ItemID))
                {
                    caster.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_CONVERT_THIS_ITEM);
                    return nothing;
                }

                int newid = ItemTable.Instance.ConvertDataList[item.Template.ItemID];

                int pdollId = player.Inventory.getPaperdollId(item.Template);
                player.setPaperdoll(pdollId, null, false);
                player.broadcastUserInfo();

                int oldweight = item.Template.Weight;
                item.Template = ItemTable.Instance.GetItem(newid);
                item.sql_update();

                if (oldweight != item.Template.Weight)
                    player.updateWeight();

                player.setPaperdoll(pdollId, item, false);
                player.broadcastUserInfo();

                InventoryUpdate iu = new InventoryUpdate();
                iu.addModItem(item);
                player.sendPacket(iu);
            }

            return nothing;
        }

        public override bool canUse(L2Character caster)
        {
            L2Item item = caster.getWeaponItem();
            if (item == null)
            {
                caster.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_CONVERT_THIS_ITEM);
                return false;
            }

            if (item.AugmentationID > 0)
            {
                caster.sendSystemMessage(SystemMessage.SystemMessageId.AUGMENTED_ITEM_CANNOT_BE_CONVERTED);
                return false;
            }

            if (!ItemTable.Instance.ConvertDataList.ContainsKey(item.Template.ItemID))
            {
                caster.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_CONVERT_THIS_ITEM);
                return false;
            }

            return true;
        }
    }
}