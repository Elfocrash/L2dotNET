using System.Collections.Generic;

namespace L2dotNET.GameService.Tables.Multisell
{
    public class MultiSellEntry
    {
        public readonly List<MultiSellItem> take = new List<MultiSellItem>();
        public readonly List<MultiSellItem> give = new List<MultiSellItem>();
        public long dutyCount;
        public byte Stackable = 1;
        public short enchant;
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