namespace L2dotNET.Models
{
    public class Location
    {
        public static Location DUMMY_LOC = new Location(0, 0, 0);

        private volatile int x;
        private volatile int y;
        private volatile int z;

        public int X
        {
            get { return x; }
        }
        public int Y
        {
            get { return y; }
        }
        public int Z
        {
            get { return z; }
        }

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

        public void Set(int locX, int locY, int locZ)
        {
            this.x = locX;
            this.y = locY;
            this.z = locZ;
        }

        public void Clear()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is Location)
            {
                Location loc = (Location)obj;
                return (loc.X.Equals(x) && loc.Y.Equals(y) && loc.Z.Equals(z));
            }
            return false;
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