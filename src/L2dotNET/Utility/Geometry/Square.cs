using L2dotNET.Models;

namespace L2dotNET.Utility.Geometry
{
    public class Square : AShape
    {
        protected int _x;
        protected int _y;

        // square side
        protected int _a;

        public Square(int x, int y, int a)
        {
            _x = x;
            _y = y;

            _a = a;
        }

        public override double GetArea()
        {
            return _a * _a;
        }

        public override Location GetRandomLocation()
        {
            return new Location(_x + Rnd.Get(_a), _y + Rnd.Get(_a), 0);
        }

        public override int GetSize()
        {
            return _a * _a;
        }

        public override double GetVolume()
        {
            return 0;
        }

        public override bool IsInside(int x, int y)
        {
            int d = x - _x;
            if ((d < 0) || (d > _a))
                return false;

            d = y - _y;
            if ((d < 0) || (d > _a))
                return false;

            return true;
        }

        public override bool IsInside(int x, int y, int z)
        {
            int d = x - _x;
            if ((d < 0) || (d > _a))
                return false;

            d = y - _y;
            if ((d < 0) || (d > _a))
                return false;

            return true;
        }
    }
}