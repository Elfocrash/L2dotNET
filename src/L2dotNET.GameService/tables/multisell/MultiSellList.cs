using System.Collections.Generic;

namespace L2dotNET.GameService.Tables.Multisell
{
    public class MultiSellList
    {
        public int Id;
        public byte Dutyf = 1;
        public byte Save = 0;
        public byte All = 1;
        public readonly List<MultiSellEntry> Container = new List<MultiSellEntry>();
    }
}