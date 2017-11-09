using L2dotNET.DataContracts;
using L2dotNET.Models;
using L2dotNET.Models.zones;

namespace L2dotNET.Utility.Geometry
{
    public class Cube : Square
    {
        // cube origin coordinates
        private readonly int _z;

        public Cube(int x, int y, int z, int a) : base(x, y, a)
        {
            _z = z;
        }

        public override double GetArea()
        {
            return 6 * A * A;
        }

        public override double GetVolume()
        {
            return A * A * A;
        }

        public override bool IsInside(int x, int y, int z)
        {
            int d;

            d = z - _z;
            if ((d < 0) || (d > A))
                return false;

            d = x - X;
            if ((d < 0) || (d > A))
                return false;

            d = y - Y;
            if ((d < 0) || (d > A))
                return false;

            return true;
        }

        public override Location GetRandomLocation()
        {
            return new Location(X + Rnd.Get(A), Y + Rnd.Get(A), _z + Rnd.Get(A));
        }
    }
}