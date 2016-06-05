using System;
using L2dotNET.Models;

namespace L2dotNET.Utility
{
    public class Circle : AShape
    {
        // circle center coordinates
        protected int _x;
        protected int _y;

        // circle radius
        protected int _r;

        public Circle(int x, int y, int r)
        {
            _x = x;
            _y = y;

            _r = r;
        }

        public override double GetArea()
        {
            return (int)Math.PI * _r * _r;
        }

        public override Location GetRandomLocation()
        {
            double distance = Math.Sqrt(Rnd.NextDouble()) * _r;
            double angle = Rnd.NextDouble() * Math.PI * 2;

            return new Location((int)(distance * Math.Cos(angle)), (int)(distance * Math.Sin(angle)), 0);
        }

        public override int GetSize()
        {
            return (int)Math.PI * _r * _r;
        }

        public override double GetVolume()
        {
            return 0;
        }

        public override bool IsInside(int x, int y)
        {
            int dx = x - _x;
            int dy = y - _y;

            return (dx * dx + dy * dy) <= _r * _r;
        }

        public override bool IsInside(int x, int y, int z)
        {
            int dx = x - _x;
            int dy = y - _y;

            return (dx * dx + dy * dy) <= _r * _r;
        }
    }
}