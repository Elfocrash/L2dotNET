using System;
using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.ItemEnchantAPI
{
    class RequestEnchantItem : GameServerNetworkRequest
    {
        private int a_sTargetID;
        private int a_sSupportID;

        public RequestEnchantItem(GameClient client, byte[] data)
        {
            makeme(client, data);
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
                player.SendSystemMessage(SystemMessage.SystemMessageId.INAPPROPRIATE_ENCHANT_CONDITION);
                player.SendActionFailed();
                return;
            }

            if (a_sTargetID != player.EnchantItem.ObjId)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.INAPPROPRIATE_ENCHANT_CONDITION);
                player.SendActionFailed();
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
            if ((rate == 100) || (new Random().Next(100) < rate))
            {
                player.EnchantItem.Enchant += 1;
                player.EnchantItem.sql_update();

                iu = new InventoryUpdate();
                iu.addModItem(player.EnchantItem);

                player.SendPacket(new EnchantResult(EnchantResultVal.success));

                equip = player.EnchantItem.IsEquipped == 1;

                if (equip && (player.EnchantItem.Enchant == 4) && (player.EnchantItem.Template.item_skill_ench4 != null))
                {
                    player.AddSkill(player.EnchantItem.Template.item_skill_ench4, false, false);
                    player.UpdateSkillList();
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

                        player.SendPacket(new EnchantResult(EnchantResultVal.breakToOne));
                        break;
                    case EnchantType.ancient:
                        player.SendPacket(new EnchantResult(EnchantResultVal.safeBreak));
                        break;
                    default:
                    {
                        if (player.EnchantItem.IsEquipped == 1)
                        {
                            //int pdollId = player.Inventory.getPaperdollId(player.EnchantItem.Template);
                           // player.setPaperdoll(pdollId, null, false);
                            equip = true;
                        }

                        iu = new InventoryUpdate();
                        iu.addDelItem(player.EnchantItem);

                        int cry = player.EnchantItem.Template._cryCount;

                        if (cry == 0)
                            player.SendPacket(new EnchantResult(EnchantResultVal.breakToNothing));
                        else
                        {
                            int id = player.EnchantItem.Template.getCrystallId();
                            player.SendPacket(new EnchantResult(EnchantResultVal.breakToCount, id, cry));
                            player.AddItem(id, cry);
                        }
                    }
                        break;
                }
            }

            if (player.EnchantStone != null)
                player.DestroyItem(player.EnchantStone, 1);

            player.DestroyItem(player.EnchantScroll, 1);

            if (iu != null)
                player.SendPacket(iu);

            player.EnchantItem = null;
            player.EnchantScroll = null;
            player.EnchantStone = null;
            player.EnchantState = 0;

            if (equip)
                player.BroadcastUserInfo();
        }
    }
}