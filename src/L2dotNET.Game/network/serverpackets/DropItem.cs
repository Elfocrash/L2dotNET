using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2send
{
    class DropItem : GameServerNetworkPacket
    {
        private int id;
        private L2Item item;
        public DropItem(L2Item item)
        {
            this.item = item;
            this.id = item._dropper;
        }

        protected internal override void write()
        {
            writeC(0x0c);
            writeD(id);
            writeD(item.ObjID);
            writeD(item.Template.ItemID);
            writeD(item.X);
            writeD(item.Y);
            writeD(item.Z);
            writeD(item.Template.isStackable() ? 1 : 0);
            writeQ(item.Count);
            writeD(1); // ?
        }
    }
}
