using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class DropItem : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly L2Item _item;

        public DropItem(L2Item item)
        {
            this._item = item;
            _id = item.Dropper;
        }

        protected internal override void Write()
        {
            WriteC(0x0c);
            WriteD(_id);
            WriteD(_item.ObjId);
            WriteD(_item.Template.ItemId);
            WriteD(_item.X);
            WriteD(_item.Y);
            WriteD(_item.Z);
            WriteD(_item.Template.IsStackable() ? 1 : 0);
            WriteQ(_item.Count);
            WriteD(1); // ?
        }
    }
}