using System.Collections.Generic;
using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2send
{
    class TradeStart : GameServerNetworkPacket
    {
        private L2Player player;
        private List<L2Item> trade = new List<L2Item>();
        private int partnerId;
        public TradeStart(L2Player player)
        {
            this.player = player;
            this.partnerId = player.requester.ObjID;
            foreach (L2Item item in player.getAllNonQuestItems())
            {
                if (item.Template.is_trade == 0 || item.AugmentationID > 0 || item._isEquipped == 1)
                    continue;

                if (item.Template.Type == ItemTemplate.L2ItemType.asset)
                    continue;

                trade.Add(item);
            }
        }

        protected internal override void write()
        {
            writeC(0x14);
            writeD(partnerId);
            writeH(trade.Count);

            foreach (L2Item item in trade)
            {
                writeD(item.ObjID);
                writeD(item.Template.ItemID);
                writeD(0);
                writeQ(item.Count);

                writeH(item.Template.Type2());
                writeH(item.CustomType1);
                writeH(0);

                writeD(item.Template.BodyPartId());
                writeH(item.Enchant);
                writeH(item.CustomType2);

                writeD(item.AugmentationID);
                writeD(item.Durability);
                writeD(item.LifeTimeEnd());

                writeH(item.AttrAttackType);
                writeH(item.AttrAttackValue);
                writeH(item.AttrDefenseValueFire);
                writeH(item.AttrDefenseValueWater);
                writeH(item.AttrDefenseValueWind);
                writeH(item.AttrDefenseValueEarth);
                writeH(item.AttrDefenseValueHoly);
                writeH(item.AttrDefenseValueUnholy);

                writeH(item.Enchant1);
                writeH(item.Enchant2);
                writeH(item.Enchant3);
            }
        }
    }
}
