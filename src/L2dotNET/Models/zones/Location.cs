namespace L2dotNET.Models.zones
{
    public class Location
    {
        public static Location DummyLoc = new Location(0, 0, 0);

        private volatile int _x;
        private volatile int _y;
        private volatile int _z;

        public int X => _x;
        public int Y => _y;
        public int Z => _z;

        public Location(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public Location(Location loc)
        {
            _x = loc.X;
            _y = loc.Y;
            _z = loc.Z;
        }

        public void Set(int locX, int locY, int locZ)
        {
            _x = locX;
            _y = locY;
            _z = locZ;
        }

        public void Clear()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Location))
                return false;

            Location loc = (Location)obj;
            return loc.X.Equals(_x) && loc.Y.Equals(_y) && loc.Z.Equals(_z);
        }

        public override int GetHashCode()
        {
            return _x ^ _y ^ _z;
        }

        public override string ToString()
        {
            return $"{_x}, {_y}, {_z}";
        }
    }
}