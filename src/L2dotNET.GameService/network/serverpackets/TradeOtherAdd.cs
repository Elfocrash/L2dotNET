using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeOtherAdd : GameServerNetworkPacket
    {
        private readonly L2Item _item;
        private long _num;

        public TradeOtherAdd(L2Item item, long num)
        {
            this._item = item;
            this._num = num;
        }

        protected internal override void Write()
        {
            WriteC(0x20);
            WriteH(1);

            WriteH(_item.Template.Type1());
            WriteD(0); //item.ObjID
            WriteD(_item.Template.ItemId);
            WriteD(_item.Count);

            WriteH(_item.Template.Type2());
            WriteH(0); // ??

            WriteD(_item.Template.BodyPartId());
            WriteH(_item.Enchant);
            WriteH(0x00); // ?
            WriteH(0x00);
        }
    }
}