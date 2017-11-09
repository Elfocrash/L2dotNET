using System;
using L2dotNET.DataContracts;
using L2dotNET.Models;
using L2dotNET.Models.zones;

namespace L2dotNET.Utility.Geometry
{
    public class Circle : AShape
    {
        // circle center coordinates
        protected int X;
        protected int Y;

        // circle radius
        protected int R;

        public Circle(int x, int y, int r)
        {
            X = x;
            Y = y;

            R = r;
        }

        public override double GetArea()
        {
            return (int)Math.PI * R * R;
        }

        public override Location GetRandomLocation()
        {
            double distance = Math.Sqrt(Rnd.NextDouble()) * R;
            double angle = Rnd.NextDouble() * Math.PI * 2;

            return new Location((int)(distance * Math.Cos(angle)), (int)(distance * Math.Sin(angle)), 0);
        }

        public override int GetSize()
        {
            return (int)Math.PI * R * R;
        }

        public override double GetVolume()
        {
            return 0;
        }

        public override bool IsInside(int x, int y)
        {
            int dx = x - X;
            int dy = y - Y;

            return ((dx * dx) + (dy * dy)) <= (R * R);
        }

        public override bool IsInside(int x, int y, int z)
        {
            int dx = x - X;
            int dy = y - Y;

            return ((dx * dx) + (dy * dy)) <= (R * R);
        }
    }
}