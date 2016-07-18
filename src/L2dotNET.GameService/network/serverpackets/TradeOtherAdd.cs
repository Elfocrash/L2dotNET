using L2dotNET.GameService.Model.Items;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeOtherAdd : GameserverPacket
    {
        private readonly L2Item _item;
        private long _num;

        public TradeOtherAdd(L2Item item, long num)
        {
            _item = item;
            _num = num;
        }

        public override void Write()
        {
            WriteByte(0x20);
            WriteShort(1);

            WriteShort(_item.Template.Type1);
            WriteInt(0); //item.ObjID
            WriteInt(_item.Template.ItemId);
            WriteInt(_item.Count);

            WriteShort(_item.Template.Type2);
            WriteShort(0); // ??

            WriteInt(_item.Template.BodyPart);
            WriteShort(_item.Enchant);
            WriteShort(0x00); // ?
            WriteShort(0x00);
        }
    }
}