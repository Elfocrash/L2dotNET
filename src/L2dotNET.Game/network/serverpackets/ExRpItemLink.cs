using L2dotNET.GameService.Model.items;

namespace L2dotNET.GameService.network.serverpackets
{
    class ExRpItemLink : GameServerNetworkPacket
    {
        private readonly L2Item item;

        public ExRpItemLink(L2Item item)
        {
            this.item = item;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x6c);

            writeD(item.ObjID);
            writeD(item.Template.ItemID);
            writeD(0);
            writeQ(item.Count);
            writeH(item.Template.Type2());
            writeH(0);
            writeH(0);
            writeD(item.Template.BodyPartId());
            writeH(item.Enchant);
            writeH(0);
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