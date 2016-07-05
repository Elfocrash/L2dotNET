using L2dotNET.Models;

namespace L2dotNET.Utility.Geometry
{
    public class Rectangle : AShape
    {
        // rectangle origin coordinates
        protected int X;
        protected int Y;

        // rectangle width and height
        protected int W;
        protected int H;

        public Rectangle(int x, int y, int w, int h)
        {
            X = x;
            Y = y;

            W = w;
            H = h;
        }

        public override double GetArea()
        {
            return W * H;
        }

        public override Location GetRandomLocation()
        {
            return new Location(X + Rnd.Get(W), Y + Rnd.Get(H), 0);
        }

        public override int GetSize()
        {
            return W * H;
        }

        public override double GetVolume()
        {
            return 0;
        }

        public override bool IsInside(int x, int y)
        {
            int d = x - X;
            if ((d < 0) || (d > W))
            {
                return false;
            }

            d = y - Y;
            if ((d < 0) || (d > H))
            {
                return false;
            }

            return true;
        }

        public override bool IsInside(int x, int y, int z)
        {
            int d = x - X;
            if ((d < 0) || (d > W))
            {
                return false;
            }

            d = y - Y;
            if ((d < 0) || (d > H))
            {
                return false;
            }

            return true;
        }
    }
}