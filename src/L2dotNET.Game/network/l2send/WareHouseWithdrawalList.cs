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
                writeD(item.ObjID);
                writeD(item.Template.ItemID);
                writeD(0);
                writeQ(item.Count);

                writeH(item.Template.Type2());
			    writeH(0);
                writeH(0); 
                writeD(item.Template.BodyPartId());
                writeH(0);
                writeH(item.Enchant);

                writeD(item.AugmentationID);
                writeD(item.Durability);
                writeD(item.LifeTimeEnd());

                writeH(item.AttrAttackType);
                writeH(item.AttrAttackValue);
                writeH(item.AttrDefenseValueFire);
                writeH(item.AttrDefenseValueWater);
                writeH(item.AttrDefenseValueWind);
                writeH(item.AttrDefenseValueEarth);
                writeH(item.AttrDefenseValueHoly);
                writeH(item.AttrDefenseValueUnholy);

                writeH(item.Enchant1);
                writeH(item.Enchant2);
                writeH(item.Enchant3);

                writeD(item.ObjID);
		    }
        }
    }
}
