using L2dotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Utility
{
    public class Cylinder : Circle
    {
        // min and max Z coorinates
        private int _minZ;
        private int _maxZ;

        public Cylinder(int x, int y, int r, int minZ, int maxZ):base(x, y, r)
        {
            _minZ = minZ;
            _maxZ = maxZ;
        }

        public override double GetArea()
        {
            return 2 * Math.PI * _r * (_r + _maxZ - _minZ);
        }

        public override Location GetRandomLocation()
        {
            // get uniform distance and angle
            double distance = Math.Sqrt(Rnd.NextDouble()) * _r;
            double angle = Rnd.NextDouble() * Math.PI * 2;

            return new Location((int)(distance * Math.Cos(angle)), (int)(distance * Math.Sin(angle)), Rnd.Get(_minZ, _maxZ));
        }

        public override double GetVolume()
        {
            return Math.PI * _r * _r * (_maxZ - _minZ);
        }

        public override bool IsInside(int x, int y, int z)
        {
            if (z < _minZ || z > _maxZ)
                return false;

            int dx = x - _x;
            int dy = y - _y;

            return (dx * dx + dy * dy) <= _r * _r;
        }
    }
}
