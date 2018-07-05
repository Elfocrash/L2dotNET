using L2dotNET.Models.Items;

namespace L2dotNET.Network.serverpackets
{
    class DropItem : GameserverPacket
    {
        private readonly int _id;
        private readonly L2Item _item;

        public DropItem(L2Item item)
        {
            _item = item;
            _id = item.Dropper;
        }

        public override void Write()
        {
            WriteByte(0x0c);
            WriteInt(_id);
            WriteInt(_item.ObjectId);
            WriteInt(_item.Template.ItemId);
            WriteInt(_item.X);
            WriteInt(_item.Y);
            WriteInt(_item.Z);
            WriteInt(_item.Template.Stackable ? 1 : 0);
            WriteLong(_item.Count);
            WriteInt(1); // ?
        }
    }
}