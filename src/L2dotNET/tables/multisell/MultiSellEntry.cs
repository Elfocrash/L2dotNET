using System.Collections.Generic;

namespace L2dotNET.tables.multisell
{
    public class MultiSellEntry
    {
        public readonly List<MultiSellItem> Take = new List<MultiSellItem>();
        public readonly List<MultiSellItem> Give = new List<MultiSellItem>();
        public int DutyCount;
        public byte Stackable = 1;
        public short Enchant;
        public short AttrAttackType = -2;
        public short AttrAttackValue;
        public short AttrDefenseValueFire;
        public short AttrDefenseValueWater;
        public short AttrDefenseValueWind;
        public short AttrDefenseValueEarth;
        public short AttrDefenseValueHoly;
        public short AttrDefenseValueUnholy;
    }
}