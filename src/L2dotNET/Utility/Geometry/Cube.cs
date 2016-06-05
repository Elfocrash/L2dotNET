using L2dotNET.Models;

namespace L2dotNET.Utility
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
            return 6 * _a * _a;
        }

        public override double GetVolume()
        {
            return _a * _a * _a;
        }

        public override bool IsInside(int x, int y, int z)
        {
            int d = z - _z;
            if (d < 0 || d > _a)
                return false;

            d = x - _x;
            if (d < 0 || d > _a)
                return false;

            d = y - _y;
            if (d < 0 || d > _a)
                return false;

            return true;
        }

        public override Location GetRandomLocation()
        {
            return new Location(_x + Rnd.Get(_a), _y + Rnd.Get(_a), _z + Rnd.Get(_a));
        }
    }
}