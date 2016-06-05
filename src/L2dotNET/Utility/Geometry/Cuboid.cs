using L2dotNET.Models;

namespace L2dotNET.Utility
{
    public class Cuboid : Rectangle
    {
        // min and max Z coorinates
        private readonly int _minZ;
        private readonly int _maxZ;

        public Cuboid(int x, int y, int minZ, int maxZ, int w, int h) : base(x, y, w, h)
        {
            _minZ = minZ;
            _maxZ = maxZ;
        }

        public override double GetArea()
        {
            return 2 * (_w * _h + (_w + _h) * (_maxZ - _minZ));
        }

        public override Location GetRandomLocation()
        {
            return new Location(_x + Rnd.Get(_w), _y + Rnd.Get(_h), Rnd.Get(_minZ, _maxZ));
        }

        public override double GetVolume()
        {
            return _w * _h * (_maxZ - _minZ);
        }

        public override bool IsInside(int x, int y, int z)
        {
            if (z < _minZ || z > _maxZ)
                return false;

            int d = x - _x;
            if (d < 0 || d > _w)
                return false;

            d = y - _y;
            if (d < 0 || d > _h)
                return false;

            return true;
        }
    }
}