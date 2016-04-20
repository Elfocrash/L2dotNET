using System.Collections.Generic;
using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2send
{
    class WareHouseWithdrawalList : GameServerNetworkPacket
    {
        public static short WH_PRIVATE = 1;
        public static short WH_CLAN = 2;
        public static short WH_CASTLE = 3;
        public static short WH_FREIGHT = 4;
        private List<L2Item> _items = new List<L2Item>();
        private short _type;
        private long _adena;
        public WareHouseWithdrawalList(L2Player player, List<L2Item> items, short type)
        {
            _type = type;
            _adena = player.getAdena();
            _items = items;
        }

        protected internal override void write()
        {
            writeC(0x42);
            writeH(_type);
            writeQ(_adena);
            writeH(_items.Count);

            foreach (L2Item item in _items) 
            {
                writeH(item.Template.Type1());
                writeD(item.ObjID);
                writeD(item.Template.ItemID);
                writeD(item.Count);
                writeH(item.Template.Type2());
                writeH(0);//custom type 1
                writeD(item.Template.BodyPartId());
                writeH(item.Enchant);
                writeH(0);//custom type 2
                writeH(0);
                //writeD(item.AugmentationID);
                writeD(item.ObjID);
                writeQ(0x00);
            }
        }
    }
}
