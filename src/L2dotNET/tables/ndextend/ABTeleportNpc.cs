using System.Collections.Generic;
using L2dotNET.tables.admin_bypass;

namespace L2dotNET.tables.ndextend
{
    public class ABTeleportNpc
    {
        public int Id;
        public SortedList<int, ABTeleportGroup> Groups = new SortedList<int, ABTeleportGroup>();
    }
}