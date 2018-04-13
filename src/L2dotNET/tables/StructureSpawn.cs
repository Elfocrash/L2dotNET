using System;

namespace L2dotNET.Tables
{
    class StructureSpawn
    {
        public int X,
                   Y,
                   Z,
                   Heading;
        public int RespawnSec = 60;
        public int NpcId;

        internal void SetLocation(string[] loc)
        {
            X = Convert.ToInt32(loc[0]);
            Y = Convert.ToInt32(loc[1]);
            Z = Convert.ToInt32(loc[2]);
            Heading = Convert.ToInt32(loc[3]);
        }
    }
}