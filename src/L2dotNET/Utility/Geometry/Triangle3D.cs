using L2dotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Utility
{
    public class Triangle3D : Triangle
    {
        private int _minZ;
        private int _maxZ;

        private double _length;

        public Triangle3D(int[] A, int[] B, int[] C) : base(A, B, C)
        {
            _minZ = Math.Min(A[2], Math.Min(B[2], C[2]));
            _maxZ = Math.Max(A[2], Math.Max(B[2], C[2]));

            int CBx = _CAx - _BAx;
            int CBy = _CAy - _BAy;
            _length = Math.Sqrt(_BAx * _BAx + _BAy * _BAy) + Math.Sqrt(_CAx * _CAx + _CAy * _CAy) + Math.Sqrt(CBx * CBx + CBy * CBy);
        }

        public override double GetArea()
        {
            return _size * 2 + _length * (_maxZ - _minZ);
        }

        public override double GetVolume()
        {
            return _size * (_maxZ - _minZ);
        }

        public override bool IsInside(int x, int y, int z)
        {
            if (z < _minZ || z > _maxZ)
                return false;

            return base.IsInside(x, y, z);
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
            
            return new Location(x, y, Rnd.Get(_minZ, _maxZ));
        }
    }
}
