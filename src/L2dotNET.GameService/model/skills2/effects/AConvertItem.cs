using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class AConvertItem : Effect
    {
        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            if (!(caster is L2Player))
                return Nothing;

            L2Player player = (L2Player)caster;
            L2Item item = player.GetWeaponItem();

            //int newid = ItemTable.Instance.ConvertDataList[item.Template.ItemID];

            //int pdollId = player.Inventory.getPaperdollId(item.Template);
            //player.setPaperdoll(pdollId, null, false);
            //player.broadcastUserInfo();

            //int oldweight = item.Template.Weight;
            //item.Template = ItemTable.Instance.GetItem(newid);
            //item.sql_update();

            //if (oldweight != item.Template.Weight)
            //    player.updateWeight();

            //player.setPaperdoll(pdollId, item, false);
            //player.broadcastUserInfo();

            InventoryUpdate iu = new InventoryUpdate();
            iu.AddModItem(item);
            player.SendPacket(iu);

            return Nothing;
        }

        public override bool CanUse(L2Character caster)
        {
            L2Item item = caster.GetWeaponItem();
            if (item == null)
            {
                caster.SendSystemMessage(SystemMessage.SystemMessageId.CannotConvertThisItem);
                return false;
            }

            if (item.AugmentationId <= 0)
                return true;

            caster.SendSystemMessage(SystemMessage.SystemMessageId.AugmentedItemCannotBeConverted);
            return false;

            //if (!ItemTable.Instance.ConvertDataList.ContainsKey(item.Template.ItemId))
            //{
            //    caster.SendSystemMessage(SystemMessage.SystemMessageId.CannotConvertThisItem);
            //    return false;
            //}
        }
    }
}