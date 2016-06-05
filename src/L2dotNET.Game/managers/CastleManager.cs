using System.Collections.Generic;
using L2dotNET.GameService.Model.structures;

namespace L2dotNET.GameService.Managers
{
    class CastleManager
    {
        private static readonly CastleManager inst = new CastleManager();

        public static CastleManager getInstance()
        {
            return inst;
        }

        public Dictionary<int, Castle> castles = new Dictionary<int, Castle>();
        private string[] announcements;

        public CastleManager()
        {
            //  announcements = File.ReadAllLines(@"data\announcements.txt");
        }

        public Castle get(int id)
        {
            if (castles.ContainsKey(id))
                return castles[id];
            else
                return null;
        }
    }
}