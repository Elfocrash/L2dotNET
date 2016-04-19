using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2send
{
    class TradeUpdate : GameServerNetworkPacket
    {
        private L2Item item;
        private long num;
        private byte action;
        public TradeUpdate(L2Item item, long num, byte action)
        {
            this.item = item;
            this.num = num;
            this.action = action;
        }

        protected internal override void write()
        {
            writeC(0x81);
            writeH(1);
            writeH(action);

            writeH(item.Template.Type1());
            writeD(item.ObjID);
            writeD(item.Template.ItemID);
            writeQ(num);

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
