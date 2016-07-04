using System.Collections.Generic;

namespace L2dotNET.GameService.Tables.Admin_Bypass
{
    public class AbTeleportGroup
    {
        public SortedList<int, AbTeleportEntry> Teles = new SortedList<int, AbTeleportEntry>();
        public int Level;
        public string Name;
        public int Id;
        public string Str;
    }
}