using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2send
{
    class TradeOtherAdd : GameServerNetworkPacket
    {
        private L2Item item;
        private long num;
        public TradeOtherAdd(L2Item item, long num)
        {
            this.item = item;
            this.num = num;
        }

        protected internal override void write()
        {
            writeC(0x1b);
            writeH(1); 

            writeH(item.Template.Type1());
            writeD(0);//item.ObjID
            writeD(item.Template.ItemID);
            writeQ(item.Count);

            writeH(item.Template.Type2());
            writeH(0);

            writeD(item.Template.BodyPartId());
            writeH(item.Enchant);
            writeH(0x00); // ?
            writeH(item.CustomType2);

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
