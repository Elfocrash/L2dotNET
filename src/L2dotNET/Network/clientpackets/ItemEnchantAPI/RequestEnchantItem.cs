using System;
using System.Threading.Tasks;
using L2dotNET.Managers;
using L2dotNET.Network.serverpackets;
using L2dotNET.Utility;

namespace L2dotNET.Network.clientpackets.ItemEnchantAPI
{
    class RequestEnchantItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _aSTargetId;
        private readonly int _aSSupportId;

        public RequestEnchantItem(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _aSTargetId = packet.ReadInt();
            _aSSupportId = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                var player = _client.CurrentPlayer;

                if (player.EnchantState != ItemEnchantManager.StateEnchantStart)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.InappropriateEnchantCondition);
                    player.SendActionFailedAsync();
                    return;
                }

                if (_aSTargetId != player.EnchantItem.CharacterId)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.InappropriateEnchantCondition);
                    player.SendActionFailedAsync();
                    return;
                }

                short rate = 50;
                if (player.EnchantStone != null)
                {
                    EnchantScroll stone = ItemEnchantManager.GetInstance()
                        .GetSupport(player.EnchantStone.Template.ItemId);
                    rate += stone.Bonus;
                }

                if (player.EnchantItem.Enchant < 4)
                    rate = 100;

                if (rate > 100)
                    rate = 100;

                InventoryUpdate iu = null;
                bool equip = false;
                if ((rate == 100) || (RandomThreadSafe.Instance.Next(100) < rate))
                {
                    player.EnchantItem.Enchant += 1;
                    //player.EnchantItem.sql_update();

                    iu = new InventoryUpdate();
                    iu.AddModItem(player.EnchantItem);

                    player.SendPacketAsync(new EnchantResult(EnchantResultVal.Success));

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
                    EnchantScroll scr = ItemEnchantManager.GetInstance()
                        .GetScroll(player.EnchantScroll.Template.ItemId);
                    switch (scr.Type)
                    {
                        case EnchantType.Blessed:
                            player.EnchantItem.Enchant = 0;
                            //player.EnchantItem.sql_update();

                            iu = new InventoryUpdate();
                            iu.AddModItem(player.EnchantItem);

                            player.SendPacketAsync(new EnchantResult(EnchantResultVal.BreakToOne));
                            break;
                        case EnchantType.Ancient:
                            player.SendPacketAsync(new EnchantResult(EnchantResultVal.SafeBreak));
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
                    player.DestroyItem(player.EnchantStone, 1);

                player.DestroyItem(player.EnchantScroll, 1);

                if (iu != null)
                    player.SendPacketAsync(iu);

                player.EnchantItem = null;
                player.EnchantScroll = null;
                player.EnchantStone = null;
                player.EnchantState = 0;

                if (equip)
                    player.BroadcastUserInfoAsync();
            });
        }
    }
}