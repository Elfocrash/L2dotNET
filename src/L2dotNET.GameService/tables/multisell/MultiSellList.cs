using System.Collections.Generic;

namespace L2dotNET.GameService.Tables.Multisell
{
    public class MultiSellList
    {
        public int id;
        public byte dutyf = 1;
        public byte save = 0;
        public byte all = 1;
        public readonly List<MultiSellEntry> container = new List<MultiSellEntry>();
    }
}