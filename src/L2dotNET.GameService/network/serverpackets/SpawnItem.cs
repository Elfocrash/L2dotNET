using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class SpawnItem : GameServerNetworkPacket
    {
        private readonly L2Item _item;

        public SpawnItem(L2Item item)
        {
            _item = item;
        }

        protected internal override void Write()
        {
            WriteC(0x0b);
            WriteD(_item.ObjId);
            WriteD(_item.Template.ItemId);
            WriteD(_item.X);
            WriteD(_item.Y);
            WriteD(_item.Z);
            WriteD(_item.Template.Stackable ? 1 : 0);
            WriteD(_item.Count);
            WriteD(0); // ?
        }
    }
}