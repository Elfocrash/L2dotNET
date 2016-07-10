using System.Collections.Generic;

namespace L2dotNET.GameService.Tables.Admin_Bypass
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