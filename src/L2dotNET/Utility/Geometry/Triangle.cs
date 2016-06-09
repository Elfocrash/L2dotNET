using System;
using L2dotNET.Models;

namespace L2dotNET.Utility.Geometry
{
    public class Triangle : AShape
    {
        // A point
        protected int _Ax;
        protected int _Ay;

        // BA vector coordinates
        protected int _BAx;
        protected int _BAy;

        // CA vector coordinates
        protected int _CAx;
        protected int _CAy;

        // size
        protected int _size;

        public Triangle(int[] A, int[] B, int[] C)
        {
            _Ax = A[0];
            _Ay = A[1];

            _BAx = B[0] - A[0];
            _BAy = B[1] - A[1];

            _CAx = C[0] - A[0];
            _CAy = C[1] - A[1];

            _size = Math.Abs(_BAx * _CAy - _CAx * _BAy) / 2;
        }

        public override double GetArea()
        {
            return _size;
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

            int x = _Ax + (int)(ba * _BAx + ca * _CAx);
            int y = _Ay + (int)(ba * _BAy + ca * _CAy);

            return new Location(x, y, 0);
        }

        public override int GetSize()
        {
            return _size;
        }

        public override double GetVolume()
        {
            return 0;
        }

        public override bool IsInside(int x, int y)
        {
            long dx = x - _Ax;
            long dy = y - _Ay;

            bool a = (0 - dx) * (_BAy - 0) - (_BAx - 0) * (0 - dy) >= 0;
            bool b = (_BAx - dx) * (_CAy - _BAy) - (_CAx - _BAx) * (_BAy - dy) >= 0;
            bool c = (_CAx - dx) * (0 - _CAy) - (0 - _CAx) * (_CAy - dy) >= 0;

            return (a == b) && (b == c);
        }

        public override bool IsInside(int x, int y, int z)
        {
            long dx = x - _Ax;
            long dy = y - _Ay;

            bool a = (0 - dx) * (_BAy - 0) - (_BAx - 0) * (0 - dy) >= 0;
            bool b = (_BAx - dx) * (_CAy - _BAy) - (_CAx - _BAx) * (_BAy - dy) >= 0;
            bool c = (_CAx - dx) * (0 - _CAy) - (0 - _CAx) * (_CAy - dy) >= 0;

            return (a == b) && (b == c);
        }
    }
}