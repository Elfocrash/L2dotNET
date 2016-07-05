using System;
using L2dotNET.Models;

namespace L2dotNET.Utility.Geometry
{
    public class Triangle3D : Triangle
    {
        private readonly int _minZ;
        private readonly int _maxZ;

        private readonly double _length;

        public Triangle3D(int[] a, int[] b, int[] c) : base(a, b, c)
        {
            _minZ = Math.Min(a[2], Math.Min(b[2], c[2]));
            _maxZ = Math.Max(a[2], Math.Max(b[2], c[2]));

            int cBx = CAx - BAx;
            int cBy = CAy - BAy;
            _length = Math.Sqrt((BAx * BAx) + (BAy * BAy)) + Math.Sqrt((CAx * CAx) + (CAy * CAy)) + Math.Sqrt((cBx * cBx) + (cBy * cBy));
        }

        public override double GetArea()
        {
            return (Size * 2) + (_length * (_maxZ - _minZ));
        }

        public override double GetVolume()
        {
            return Size * (_maxZ - _minZ);
        }

        public override bool IsInside(int x, int y, int z)
        {
            if ((z < _minZ) || (z > _maxZ))
            {
                return false;
            }

            return base.IsInside(x, y, z);
        }

        public override Location GetRandomLocation()
        {
            double ba = Rnd.NextDouble();
            double ca = Rnd.NextDouble();

            if ((ba + ca) > 1)
            {
                ba = 1 - ba;
                ca = 1 - ca;
            }

            int x = Ax + (int)((ba * BAx) + (ca * CAx));
            int y = Ay + (int)((ba * BAy) + (ca * CAy));

            return new Location(x, y, Rnd.Get(_minZ, _maxZ));
        }
    }
}