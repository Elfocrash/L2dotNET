using System;

namespace L2dotNET.GameService.model.zones.forms
{
    public class ZoneCylinder : ZoneForm 
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _z1;
        private readonly int _z2;
        private readonly int _rad;
        private readonly int _radS;

        public ZoneCylinder(int x, int y, int z1, int z2, int rad)
        {
            _x = x;
            _y = y;
            _z1 = z1;
            _z2 = z2;
            _rad = rad;
            _radS = rad * rad;
        }

        public override bool isInsideZone(int x, int y, int z)
        {
            if ((Math.Pow(_x - x, 2) + Math.Pow(_y - y, 2)) > _radS || z < _z1 || z > _z2)
                return false;

            return true;
        }

        public bool isInsideZone(int x, int y)
        {
            if ((Math.Pow(_x - x, 2) + Math.Pow(_y - y, 2)) > _radS)
                return false;

            return true;
        }

        public bool intersectsRectangle(int ax1, int ax2, int ay1, int ay2)
        {
            if (_x > ax1 && _x < ax2 && _y > ay1 && _y < ay2)
                return true;

            if ((Math.Pow(ax1 - _x, 2) + Math.Pow(ay1 - _y, 2)) < _radS)
                return true;
            if ((Math.Pow(ax1 - _x, 2) + Math.Pow(ay2 - _y, 2)) < _radS)
                return true;
            if ((Math.Pow(ax2 - _x, 2) + Math.Pow(ay1 - _y, 2)) < _radS)
                return true;
            if ((Math.Pow(ax2 - _x, 2) + Math.Pow(ay2 - _y, 2)) < _radS)
                return true;

            if (_x > ax1 && _x < ax2)
            {
                if (Math.Abs(_y - ay2) < _rad)
                    return true;
                if (Math.Abs(_y - ay1) < _rad)
                    return true;
            }
            if (_y > ay1 && _y < ay2)
            {
                if (Math.Abs(_x - ax2) < _rad)
                    return true;
                if (Math.Abs(_x - ax1) < _rad)
                    return true;
            }

            return false;
        }

        public override double getDistanceToZone(int x, int y)
        {
            return (Math.Sqrt((Math.Pow(_x - x, 2) + Math.Pow(_y - y, 2))) - _rad);
        }

        public override int getLowZ()
        {
            return _z1;
        }

        public override int getHighZ()
        {
            return _z2;
        }
    }
}
