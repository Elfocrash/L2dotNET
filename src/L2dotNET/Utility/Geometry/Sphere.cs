using L2dotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Utility
{
    public class Sphere : Circle
    {
        private int _z;

        public Sphere(int x, int y, int z, int r) : base(x, y, r)
        {
            _z = z;
        }

        public override double GetArea()
        {
            return 4 * Math.PI * _r * _r;
        }

        public override Location GetRandomLocation()
        {
            // get uniform distance and angles
            double r = Math.Ceiling(Math.Pow((Rnd.NextDouble()) * _r, (double)1 / 3));
            double phi = Rnd.NextDouble() * 2 * Math.PI;
            double theta = Math.Acos(2 * Rnd.NextDouble() - 1);

            // calculate coordinates
            int x = (int)(_x + (r * Math.Cos(phi) * Math.Sin(theta)));
            int y = (int)(_y + (r * Math.Sin(phi) * Math.Sin(theta)));
            int z = (int)(_z + (r * Math.Cos(theta)));

            // return
            return new Location(x, y, z);
        }

        public override double GetVolume()
        {
            return (4 * Math.PI * _r * _r * _r) / 3;
        }

        public override bool IsInside(int x, int y, int z)
        {
            int dx = x - _x;
            int dy = y - _y;
            int dz = z - _z;

            return (dx * dx + dy * dy + dz * dz) <= _r * _r;
        }
    }
}
