using System.Collections.Generic;
using L2dotNET.GameService.Model.Structures;

namespace L2dotNET.GameService.Managers
{
    class CastleManager
    {
        private static readonly CastleManager Inst = new CastleManager();

        public static CastleManager GetInstance()
        {
            return Inst;
        }

        public Dictionary<int, Castle> Castles = new Dictionary<int, Castle>();
        //private string[] announcements;

        public Castle Get(int id)
        {
            return Castles.ContainsKey(id) ? Castles[id] : null;
        }
    }
}