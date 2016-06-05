using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Tables.Multisell;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class MultiSellListEx : GameServerNetworkPacket
    {
        private readonly MultiSellList list;

        public MultiSellListEx(L2Player player, MultiSellList list)
        {
            this.list = list;
        }

        protected internal override void write()
        {
            writeC(0xd0);
            writeD(list.id);
            writeD(1); // page
            writeD(1); // finished
            writeD(list.container.Count); // size of pages 40
            writeD(list.container.Count);

            int inc = 0;
            foreach (MultiSellEntry entry in list.container)
            {
                writeD(inc);
                inc++;
                writeC(entry.Stackable);
                writeH(entry.enchant);
                writeD(0x00); // C6
                writeD(0x00); // T1

                writeH(entry.AttrAttackType);
                writeH(entry.AttrAttackValue);
                writeH(entry.AttrDefenseValueFire);
                writeH(entry.AttrDefenseValueWater);
                writeH(entry.AttrDefenseValueWind);
                writeH(entry.AttrDefenseValueEarth);
                writeH(entry.AttrDefenseValueHoly);
                writeH(entry.AttrDefenseValueUnholy);

                writeH(entry.give.Count);
                writeH(entry.take.Count);

                foreach (MultiSellItem item in entry.give)
                {
                    writeD(item.ItemID);
                    writeD(item.BodyPartId);
                    writeH(item.Type2);
                    writeQ(item.count);
                    writeH(item.enchant);
                    writeD(item.augment);
                    writeD(item.Durability);
                    writeH(item.AttrAttackType);
                    writeH(item.AttrAttackValue);
                    writeH(item.AttrDefenseValueFire);
                    writeH(item.AttrDefenseValueWater);
                    writeH(item.AttrDefenseValueWind);
                    writeH(item.AttrDefenseValueEarth);
                    writeH(item.AttrDefenseValueHoly);
                    writeH(item.AttrDefenseValueUnholy);
                }

                foreach (MultiSellItem item in entry.take)
                {
                    writeD(item.ItemID);
                    writeH(item.Type2);
                    writeQ(item.count);
                    writeH(item.enchant);
                    writeD(item.augment);
                    writeD(item.Durability);
                    writeH(item.AttrAttackType);
                    writeH(item.AttrAttackValue);
                    writeH(item.AttrDefenseValueFire);
                    writeH(item.AttrDefenseValueWater);
                    writeH(item.AttrDefenseValueWind);
                    writeH(item.AttrDefenseValueEarth);
                    writeH(item.AttrDefenseValueHoly);
                    writeH(item.AttrDefenseValueUnholy);
                }
            }
        }
    }
}