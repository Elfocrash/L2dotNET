using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeOwnAdd : GameserverPacket
    {
        private readonly L2Item _item;
        private readonly long _num;

        public TradeOwnAdd(L2Item item, long num)
        {
            _item = item;
            _num = num;
        }

        protected internal override void Write()
        {
            WriteByte(0x1a);
            WriteShort(0x20);

            WriteShort(_item.Template.Type1);
            WriteInt(_item.ObjId); //item.ObjID
            WriteInt(_item.Template.ItemId);
            WriteInt(_num);

            WriteShort(_item.Template.Type2);
            WriteShort(0);

            WriteInt(_item.Template.BodyPart);
            WriteShort(_item.Enchant);
            WriteShort(0x00); // ?
            WriteShort(0x00);
        }
    }
}