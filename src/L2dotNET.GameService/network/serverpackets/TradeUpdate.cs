using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeUpdate : GameServerNetworkPacket
    {
        private readonly L2Item item;
        private readonly long num;
        private readonly byte action;

        public TradeUpdate(L2Item item, long num, byte action)
        {
            this.item = item;
            this.num = num;
            this.action = action;
        }

        protected internal override void write()
        {
            writeC(0x74);
            writeH(1);
            writeH(action);

            writeH(item.Template.Type1());
            writeD(item.ObjId);
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