using L2dotNET.Models;

namespace L2dotNET.Utility.Geometry
{
    public class Square : AShape
    {
        protected int X;
        protected int Y;

        // square side
        protected int A;

        public Square(int x, int y, int a)
        {
            X = x;
            Y = y;

            A = a;
        }

        public override double GetArea()
        {
            return A * A;
        }

        public override Location GetRandomLocation()
        {
            return new Location(X + Rnd.Get(A), Y + Rnd.Get(A), 0);
        }

        public override int GetSize()
        {
            return A * A;
        }

        public override double GetVolume()
        {
            return 0;
        }

        public override bool IsInside(int x, int y)
        {
            int d;

            d = x - X;
            if ((d < 0) || (d > A))
                return false;

            d = y - Y;
            if ((d < 0) || (d > A))
                return false;

            return true;
        }

        public override bool IsInside(int x, int y, int z)
        {
            int d;

            d = x - X;
            if ((d < 0) || (d > A))
                return false;

            d = y - Y;
            if ((d < 0) || (d > A))
                return false;

            return true;
        }
    }
}