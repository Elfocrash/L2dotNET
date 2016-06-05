using System.Collections.Generic;
using L2dotNET.GameService.Tables.Admin_Bypass;

namespace L2dotNET.GameService.Tables.Ndextend
{
    public class ab_teleport_npc
    {
        public int id;
        public SortedList<int, ab_teleport_group> groups = new SortedList<int, ab_teleport_group>();
    }
}