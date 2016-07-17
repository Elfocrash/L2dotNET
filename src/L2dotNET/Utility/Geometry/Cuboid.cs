using L2dotNET.Models;

namespace L2dotNET.Utility.Geometry
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
            return 2 * ((W * H) + ((W + H) * (_maxZ - _minZ)));
        }

        public override Location GetRandomLocation()
        {
            return new Location(X + Rnd.Get(W), Y + Rnd.Get(H), Rnd.Get(_minZ, _maxZ));
        }

        public override double GetVolume()
        {
            return W * H * (_maxZ - _minZ);
        }

        public override bool IsInside(int x, int y, int z)
        {
            if ((z < _minZ) || (z > _maxZ))
                return false;

            int d;

            d = x - X;
            if ((d < 0) || (d > W))
                return false;

            d = y - Y;
            if ((d < 0) || (d > H))
                return false;

            return true;
        }
    }
}