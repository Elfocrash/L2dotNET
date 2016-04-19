using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using L2dotNET.Game.logger;

namespace L2dotNET.Game.geo
{
    public class Geo
    {
        public virtual void loadGeo()
        {
            CLogger.warning("Geodata disabled.");
        }

        public virtual short getType(int x, int y)
        {
            return -2;
        }
    }
}
