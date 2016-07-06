using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeUpdate : GameServerNetworkPacket
    {
        private readonly L2Item _item;
        private readonly long _num;
        private readonly byte _action;

        public TradeUpdate(L2Item item, long num, byte action)
        {
            _item = item;
            _num = num;
            _action = action;
        }

        protected internal override void Write()
        {
            WriteC(0x74);
            WriteH(1);
            WriteH(_action);

            WriteH(_item.Template.Type1);
            WriteD(_item.ObjId);
            WriteD(_item.Template.ItemId);
            WriteD(_num);

            WriteH(_item.Template.Type2);
            WriteH(0);

            WriteD(_item.Template.BodyPart);
            WriteH(_item.Enchant);
            WriteH(0x00); // ?
            WriteH(0x00);
        }
    }
}