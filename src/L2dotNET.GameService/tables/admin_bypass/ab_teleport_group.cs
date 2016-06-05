using System.Collections.Generic;

namespace L2dotNET.GameService.Tables.Admin_Bypass
{
    public class ab_teleport_group
    {
        public SortedList<int, ab_teleport_entry> _teles = new SortedList<int, ab_teleport_entry>();
        public int level;
        public string name;
        public int id;
        public string str;
    }
}