using L2dotNET.Models.Items;

namespace L2dotNET.Network.serverpackets
{
    class TradeUpdate : GameserverPacket
    {
        private readonly L2Item _item;
        private readonly int _num;
        private readonly byte _action;

        public TradeUpdate(L2Item item, int num, byte action)
        {
            _item = item;
            _num = num;
            _action = action;
        }

        public override void Write()
        {
            WriteByte(0x74);
            WriteShort(1);
            WriteShort(_action);

            WriteShort(_item.Template.Type1);
            WriteInt(_item.ObjectId);
            WriteInt(_item.Template.ItemId);
            WriteInt(_num);

            WriteShort(_item.Template.Type2);
            WriteShort(0);

            WriteInt((int) _item.Template.BodyPart);
            WriteShort(_item.Enchant);
            WriteShort(0x00); // ?
            WriteShort(0x00);
        }
    }
}