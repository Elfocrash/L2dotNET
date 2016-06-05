using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.Models;

namespace L2dotNET.Utility
{
    public class Rectangle : AShape
    {
        // rectangle origin coordinates
        protected int _x;
        protected int _y;

        // rectangle width and height
        protected int _w;
        protected int _h;

        public Rectangle(int x, int y, int w, int h)
        {
            _x = x;
            _y = y;

            _w = w;
            _h = h;
        }

        public override double GetArea()
        {
            return _w * _h;
        }

        public override Location GetRandomLocation()
        {
            return new Location(_x + Rnd.Get(_w), _y + Rnd.Get(_h), 0);
        }

        public override int GetSize()
        {
            return _w * _h;
        }

        public override double GetVolume()
        {
            return 0;
        }

        public override bool IsInside(int x, int y)
        {
            int d = x - _x;
            if (d < 0 || d > _w)
                return false;

            d = y - _y;
            if (d < 0 || d > _h)
                return false;

            return true;
        }

        public override bool IsInside(int x, int y, int z)
        {
            int d = x - _x;
            if (d < 0 || d > _w)
                return false;

            d = y - _y;
            if (d < 0 || d > _h)
                return false;

            return true;
        }
    }
}
