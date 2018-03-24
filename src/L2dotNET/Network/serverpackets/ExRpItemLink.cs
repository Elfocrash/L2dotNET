using L2dotNET.Models.items;

namespace L2dotNET.Network.serverpackets
{
    class ExRpItemLink : GameserverPacket
    {
        private readonly L2Item _item;

        public ExRpItemLink(L2Item item)
        {
            _item = item;
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x6c);

            WriteInt(_item.ObjId);
            WriteInt(_item.Template.ItemId);
            WriteInt(0);
            WriteLong(_item.Count);
            WriteShort(_item.Template.Type2);
            WriteShort(0);
            WriteShort(0);
            WriteInt(_item.Template.BodyPart);
            WriteShort(_item.Enchant);
            WriteShort(0);
            WriteInt(_item.AugmentationId);
            WriteInt(_item.Durability);
            WriteInt(_item.LifeTimeEnd());

            WriteShort(_item.AttrAttackType);
            WriteShort(_item.AttrAttackValue);

        }
    }
}