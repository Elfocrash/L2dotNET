using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExRpItemLink : GameServerNetworkPacket
    {
        private readonly L2Item _item;

        public ExRpItemLink(L2Item item)
        {
            this._item = item;
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0x6c);

            WriteD(_item.ObjId);
            WriteD(_item.Template.ItemId);
            WriteD(0);
            WriteQ(_item.Count);
            WriteH(_item.Template.Type2());
            WriteH(0);
            WriteH(0);
            WriteD(_item.Template.BodyPartId());
            WriteH(_item.Enchant);
            WriteH(0);
            WriteD(_item.AugmentationId);
            WriteD(_item.Durability);
            WriteD(_item.LifeTimeEnd());

            WriteH(_item.AttrAttackType);
            WriteH(_item.AttrAttackValue);
            WriteH(_item.AttrDefenseValueFire);
            WriteH(_item.AttrDefenseValueWater);
            WriteH(_item.AttrDefenseValueWind);
            WriteH(_item.AttrDefenseValueEarth);
            WriteH(_item.AttrDefenseValueHoly);
            WriteH(_item.AttrDefenseValueUnholy);

            WriteH(_item.Enchant1);
            WriteH(_item.Enchant2);
            WriteH(_item.Enchant3);
        }
    }
}