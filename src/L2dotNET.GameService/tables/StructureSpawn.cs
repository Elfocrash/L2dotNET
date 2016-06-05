using System;

namespace L2dotNET.GameService.Tables
{
    class StructureSpawn
    {
        public int x,
                   y,
                   z,
                   heading;
        public int respawnSec = 60;
        public int npcId;

        internal void SetLocation(string[] loc)
        {
            x = Convert.ToInt32(loc[0]);
            y = Convert.ToInt32(loc[1]);
            z = Convert.ToInt32(loc[2]);
            heading = Convert.ToInt32(loc[3]);
        }
    }
}