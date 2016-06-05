using L2dotNET.GameService.model.items;

namespace L2dotNET.GameService.network.l2send
{
    class TradeOtherAdd : GameServerNetworkPacket
    {
        private readonly L2Item item;
        private long num;
        public TradeOtherAdd(L2Item item, long num)
        {
            this.item = item;
            this.num = num;
        }

        protected internal override void write()
        {
            writeC(0x20);
            writeH(1); 

            writeH(item.Template.Type1());
            writeD(0);//item.ObjID
            writeD(item.Template.ItemID);
            writeD(item.Count);

            writeH(item.Template.Type2());
            writeH(0); // ??

            writeD(item.Template.BodyPartId());
            writeH(item.Enchant);
            writeH(0x00); // ?
            writeH(0x00);
        }
    }
}
