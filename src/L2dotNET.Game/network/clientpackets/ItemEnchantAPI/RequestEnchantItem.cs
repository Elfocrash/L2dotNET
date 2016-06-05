using System;
using L2dotNET.GameService.managers;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestEnchantItem : GameServerNetworkRequest
    {
        private int a_sTargetID;
        private int a_sSupportID;

        public RequestEnchantItem(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            a_sTargetID = readD();
            a_sSupportID = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.EnchantState != ItemEnchantManager.STATE_ENCHANT_START)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.INAPPROPRIATE_ENCHANT_CONDITION);
                player.sendActionFailed();
                return;
            }

            if (a_sTargetID != player.EnchantItem.ObjID)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.INAPPROPRIATE_ENCHANT_CONDITION);
                player.sendActionFailed();
                return;
            }

            short rate = 50;
            if (player.EnchantStone != null)
            {
                EnchantScroll stone = ItemEnchantManager.getInstance().getSupport(player.EnchantStone.Template.ItemID);
                rate += stone.bonus;
            }

            if (player.EnchantItem.Enchant < 4)
                rate = 100;

            if (rate > 100)
                rate = 100;

            InventoryUpdate iu = null;
            bool equip = false;
            if (rate == 100 ? true : new Random().Next(100) < rate)
            {
                player.EnchantItem.Enchant += 1;
                player.EnchantItem.sql_update();

                iu = new InventoryUpdate();
                iu.addModItem(player.EnchantItem);

                player.sendPacket(new EnchantResult(EnchantResultVal.success));

                equip = player.EnchantItem._isEquipped == 1;

                if (equip && player.EnchantItem.Enchant == 4 && player.EnchantItem.Template.item_skill_ench4 != null)
                {
                    player.addSkill(player.EnchantItem.Template.item_skill_ench4, false, false);
                    player.updateSkillList();
                }
                //todo check +6 set
            }
            else
            {
                EnchantScroll scr = ItemEnchantManager.getInstance().getScroll(player.EnchantScroll.Template.ItemID);
                switch (scr.Type)
                {
                    case EnchantType.blessed:
                        player.EnchantItem.Enchant = 0;
                        player.EnchantItem.sql_update();

                        iu = new InventoryUpdate();
                        iu.addModItem(player.EnchantItem);

                        player.sendPacket(new EnchantResult(EnchantResultVal.breakToOne));
                        break;
                    case EnchantType.ancient:
                        player.sendPacket(new EnchantResult(EnchantResultVal.safeBreak));
                        break;
                    default:
                    {
                        if (player.EnchantItem._isEquipped == 1)
                        {
                            int pdollId = player.Inventory.getPaperdollId(player.EnchantItem.Template);
                            player.setPaperdoll(pdollId, null, false);
                            equip = true;
                        }

                        player.Inventory.removeItem(player.EnchantItem);
                        iu = new InventoryUpdate();
                        iu.addDelItem(player.EnchantItem);

                        long cry = player.EnchantItem.Template._cryCount;

                        if (cry == 0)
                            player.sendPacket(new EnchantResult(EnchantResultVal.breakToNothing));
                        else
                        {
                            int id = player.EnchantItem.Template.getCrystallId();
                            player.sendPacket(new EnchantResult(EnchantResultVal.breakToCount, id, cry));
                            player.Inventory.addItem(id, cry, true, true);
                        }
                    }
                        break;
                }
            }

            if (player.EnchantStone != null)
                player.Inventory.destroyItem(player.EnchantStone, 1, true, true);

            player.Inventory.destroyItem(player.EnchantScroll, 1, false, true);

            if (iu != null)
                player.sendPacket(iu);

            player.EnchantItem = null;
            player.EnchantScroll = null;
            player.EnchantStone = null;
            player.EnchantState = 0;

            if (equip)
                player.broadcastUserInfo();
        }
    }
}