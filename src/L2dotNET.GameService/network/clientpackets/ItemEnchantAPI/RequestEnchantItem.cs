using System;
using L2dotNET.GameService.Config;
using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.ItemEnchantAPI
{
    class RequestEnchantItem : PacketBase
    {
        private int _aSTargetId;
        private int _aSSupportId;
        private readonly GameClient _client;

        public RequestEnchantItem(Packet packet, GameClient client)
        {
            _client = client;
            _aSTargetId = packet.ReadInt();
            _aSSupportId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.EnchantState != ItemEnchantManager.StateEnchantStart)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.InappropriateEnchantCondition);
                player.SendActionFailed();
                return;
            }

            if (_aSTargetId != player.EnchantItem.ObjId)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.InappropriateEnchantCondition);
                player.SendActionFailed();
                return;
            }

            short rate = 50;
            if (player.EnchantStone != null)
            {
                EnchantScroll stone = ItemEnchantManager.GetInstance().GetSupport(player.EnchantStone.Template.ItemId);
                rate += stone.Bonus;
            }

            if (player.EnchantItem.Enchant < 4)
            {
                rate = 100;
            }

            if (rate > 100)
            {
                rate = 100;
            }

            InventoryUpdate iu = null;
            bool equip = false;
            if ((rate == 100) || (new Random().Next(100) < rate))
            {
                player.EnchantItem.Enchant += 1;
                player.EnchantItem.sql_update();

                iu = new InventoryUpdate();
                iu.AddModItem(player.EnchantItem);

                player.SendPacket(new EnchantResult(EnchantResultVal.Success));

                equip = player.EnchantItem.IsEquipped == 1;

                //if (equip && (player.EnchantItem.Enchant == 4) && (player.EnchantItem.Template.ItemSkillEnch4 != null))
                //{
                //    player.AddSkill(player.EnchantItem.Template.ItemSkillEnch4, false, false);
                //    player.UpdateSkillList();
                //}
                //todo check +6 set
            }
            else
            {
                EnchantScroll scr = ItemEnchantManager.GetInstance().GetScroll(player.EnchantScroll.Template.ItemId);
                switch (scr.Type)
                {
                    case EnchantType.Blessed:
                        player.EnchantItem.Enchant = 0;
                        player.EnchantItem.sql_update();

                        iu = new InventoryUpdate();
                        iu.AddModItem(player.EnchantItem);

                        player.SendPacket(new EnchantResult(EnchantResultVal.BreakToOne));
                        break;
                    case EnchantType.Ancient:
                        player.SendPacket(new EnchantResult(EnchantResultVal.SafeBreak));
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
                        iu.AddDelItem(player.EnchantItem);

                        //int id = player.EnchantItem.Template.CrystalType.CrystalId;
                        //player.SendPacket(new EnchantResult(EnchantResultVal.BreakToCount, id, cry));
                        //player.AddItem(id, cry);
                    }
                        break;
                }
            }

            if (player.EnchantStone != null)
            {
                player.DestroyItem(player.EnchantStone, 1);
            }

            player.DestroyItem(player.EnchantScroll, 1);

            if (iu != null)
            {
                player.SendPacket(iu);
            }

            player.EnchantItem = null;
            player.EnchantScroll = null;
            player.EnchantStone = null;
            player.EnchantState = 0;

            if (equip)
            {
                player.BroadcastUserInfo();
            }
        }
    }
}