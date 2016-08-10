using System.Collections.Generic;

namespace L2dotNET.tables.admin_bypass
{
    public class ABTeleportGroup
    {
        public SortedList<int, ABTeleportEntry> Teles = new SortedList<int, ABTeleportEntry>();
        public int Level;
        public string Name;
        public int Id;
        public string Str;
    }
}