using System;
using L2dotNET.Models;

namespace L2dotNET.Utility.Geometry
{
    public class Triangle : AShape
    {
        // A point
        protected int Ax;
        protected int Ay;

        // BA vector coordinates
        protected int BAx;
        protected int BAy;

        // CA vector coordinates
        protected int CAx;
        protected int CAy;

        // size
        protected int Size;

        public Triangle(int[] a, int[] b, int[] c)
        {
            Ax = a[0];
            Ay = a[1];

            BAx = b[0] - a[0];
            BAy = b[1] - a[1];

            CAx = c[0] - a[0];
            CAy = c[1] - a[1];

            Size = Math.Abs(BAx * CAy - CAx * BAy) / 2;
        }

        public override double GetArea()
        {
            return Size;
        }

        public override Location GetRandomLocation()
        {
            double ba = Rnd.NextDouble();
            double ca = Rnd.NextDouble();

            if (ba + ca > 1)
            {
                ba = 1 - ba;
                ca = 1 - ca;
            }

            int x = Ax + (int)(ba * BAx + ca * CAx);
            int y = Ay + (int)(ba * BAy + ca * CAy);

            return new Location(x, y, 0);
        }

        public override int GetSize()
        {
            return Size;
        }

        public override double GetVolume()
        {
            return 0;
        }

        public override bool IsInside(int x, int y)
        {
            long dx = x - Ax;
            long dy = y - Ay;

            bool a = (0 - dx) * (BAy - 0) - (BAx - 0) * (0 - dy) >= 0;
            bool b = (BAx - dx) * (CAy - BAy) - (CAx - BAx) * (BAy - dy) >= 0;
            bool c = (CAx - dx) * (0 - CAy) - (0 - CAx) * (CAy - dy) >= 0;

            return (a == b) && (b == c);
        }

        public override bool IsInside(int x, int y, int z)
        {
            long dx = x - Ax;
            long dy = y - Ay;

            bool a = (0 - dx) * (BAy - 0) - (BAx - 0) * (0 - dy) >= 0;
            bool b = (BAx - dx) * (CAy - BAy) - (CAx - BAx) * (BAy - dy) >= 0;
            bool c = (CAx - dx) * (0 - CAy) - (0 - CAx) * (CAy - dy) >= 0;

            return (a == b) && (b == c);
        }
    }
}