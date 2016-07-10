using System.Collections.Generic;
using L2dotNET.GameService.Tables.Admin_Bypass;

namespace L2dotNET.GameService.Tables.Ndextend
{
    public class ABTeleportNpc
    {
        public int Id;
        public SortedList<int, ABTeleportGroup> Groups = new SortedList<int, ABTeleportGroup>();
    }
}