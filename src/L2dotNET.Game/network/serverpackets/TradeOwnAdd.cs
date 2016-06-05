using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeOwnAdd : GameServerNetworkPacket
    {
        private readonly L2Item item;
        private readonly long num;

        public TradeOwnAdd(L2Item item, long num)
        {
            this.item = item;
            this.num = num;
        }

        protected internal override void write()
        {
            writeC(0x1a);
            writeH(0x20);

            writeH(item.Template.Type1());
            writeD(item.ObjID); //item.ObjID
            writeD(item.Template.ItemID);
            writeD(num);

            writeH(item.Template.Type2());
            writeH(0);

            writeD(item.Template.BodyPartId());
            writeH(item.Enchant);
            writeH(0x00); // ?
            writeH(0x00);
        }
    }
}