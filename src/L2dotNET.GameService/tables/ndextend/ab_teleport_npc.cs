using System.Collections.Generic;
using L2dotNET.GameService.Tables.Admin_Bypass;

namespace L2dotNET.GameService.Tables.Ndextend
{
    public class AbTeleportNpc
    {
        public int Id;
        public SortedList<int, AbTeleportGroup> Groups = new SortedList<int, AbTeleportGroup>();
    }
}