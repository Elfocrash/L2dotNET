using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GameService.model
{
    public class Location
    {
        public static Location DUMMY_LOC = new Location(0, 0, 0);

        private volatile int x;
        private volatile int y;
        private volatile int z;

        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int Z { get { return z; } }

        public Location(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Location(Location loc)
        {
            this.x = loc.X;
            this.y = loc.Y;
            this.z = loc.Z;
        }

        public void Set(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Clear()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public override bool Equals(object obj)
        {
            if(obj is Location)
            {
                Location loc = (Location)obj;
                return (loc.X.Equals(x) && loc.Y.Equals(y) && loc.Z.Equals(z));
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return x ^ y ^ z;
        }

        public override string ToString()
        {
            return x + ", " + y + ", " + z;
        }
    }
}
