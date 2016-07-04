using System;
using L2dotNET.Models;

namespace L2dotNET.Utility.Geometry
{
    public class Sphere : Circle
    {
        private readonly int _z;

        public Sphere(int x, int y, int z, int r) : base(x, y, r)
        {
            _z = z;
        }

        public override double GetArea()
        {
            return 4 * Math.PI * R * R;
        }

        public override Location GetRandomLocation()
        {
            // get uniform distance and angles
            double r = Math.Ceiling(Math.Pow((Rnd.NextDouble()) * R, (double)1 / 3));
            double phi = Rnd.NextDouble() * 2 * Math.PI;
            double theta = Math.Acos(2 * Rnd.NextDouble() - 1);

            // calculate coordinates
            int x = (int)(X + (r * Math.Cos(phi) * Math.Sin(theta)));
            int y = (int)(Y + (r * Math.Sin(phi) * Math.Sin(theta)));
            int z = (int)(_z + (r * Math.Cos(theta)));

            // return
            return new Location(x, y, z);
        }

        public override double GetVolume()
        {
            return (4 * Math.PI * R * R * R) / 3;
        }

        public override bool IsInside(int x, int y, int z)
        {
            int dx = x - X;
            int dy = y - Y;
            int dz = z - _z;

            return (dx * dx + dy * dy + dz * dz) <= R * R;
        }
    }
}