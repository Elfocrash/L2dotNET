using System;
using L2dotNET.DataContracts;
using L2dotNET.Models;
using L2dotNET.Models.zones;

namespace L2dotNET.Utility.Geometry
{
    public class Cylinder : Circle
    {
        // min and max Z coorinates
        private readonly int _minZ;
        private readonly int _maxZ;

        public Cylinder(int x, int y, int r, int minZ, int maxZ) : base(x, y, r)
        {
            _minZ = minZ;
            _maxZ = maxZ;
        }

        public override double GetArea()
        {
            return 2 * Math.PI * R * ((R + _maxZ) - _minZ);
        }

        public override Location GetRandomLocation()
        {
            // get uniform distance and angle
            double distance = Math.Sqrt(Rnd.NextDouble()) * R;
            double angle = Rnd.NextDouble() * Math.PI * 2;

            return new Location((int)(distance * Math.Cos(angle)), (int)(distance * Math.Sin(angle)), Rnd.Get(_minZ, _maxZ));
        }

        public override double GetVolume()
        {
            return Math.PI * R * R * (_maxZ - _minZ);
        }

        public override bool IsInside(int x, int y, int z)
        {
            if ((z < _minZ) || (z > _maxZ))
                return false;

            int dx = x - X;
            int dy = y - Y;

            return ((dx * dx) + (dy * dy)) <= (R * R);
        }
    }
}