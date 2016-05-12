using System.Collections.Generic;
using System.IO;
using L2dotNET.GameService.model.player.basic;
using L2dotNET.GameService.model.structures;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.managers
{
    class CastleManager
    {
        private static CastleManager inst = new CastleManager();

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
