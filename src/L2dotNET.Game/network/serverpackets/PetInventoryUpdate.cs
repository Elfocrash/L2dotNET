using System.Collections.Generic;
using L2dotNET.GameService.Model.items;

namespace L2dotNET.GameService.network.serverpackets
{
    class PetInventoryUpdate : GameServerNetworkPacket
    {
        protected List<object[]> _update = new List<object[]>();

        public void addNewItem(L2Item item)
        {
            _update.Add(new object[] { item, (short)1 });
        }

        public void addModItem(L2Item item)
        {
            _update.Add(new object[] { item, (short)2 });
        }

        public void addDelItem(L2Item item)
        {
            _update.Add(new object[] { item, (short)3 });
        }

        protected internal override void write()
        {
            writeC(0xb4);
            writeH(_update.Count);

            foreach (object[] obj in _update)
            {
                writeH((short)obj[1]);

                L2Item item = (L2Item)obj[0];

                writeD(item.ObjID);
                writeD(item.Template.ItemID);
                writeD(0); //loc
                writeQ(item.Count);

                writeH(item.Template.Type2());
                writeH(0);
                writeH(item._isEquipped);

                writeD(item.Template.BodyPartId());
                writeH(item.Enchant);
                writeH(0);

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
            }
        }
    }
}