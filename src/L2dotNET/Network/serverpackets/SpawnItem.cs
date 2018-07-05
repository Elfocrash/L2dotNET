using L2dotNET.Models.Items;

namespace L2dotNET.Network.serverpackets
{
    class SpawnItem : GameserverPacket
    {
        private readonly L2Item _item;

        public SpawnItem(L2Item item)
        {
            _item = item;
        }

        public override void Write()
        {
            WriteByte(0x0b);
            WriteInt(_item.ObjectId);
            WriteInt(_item.Template.ItemId);
            WriteInt(_item.X);
            WriteInt(_item.Y);
            WriteInt(_item.Z);
            WriteInt(_item.Template.Stackable ? 1 : 0);
            WriteInt(_item.Count);
            WriteInt(0); // ?
        }
    }
}