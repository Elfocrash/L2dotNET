using System;

namespace L2dotNET.model.zones.forms
{
    public class ZoneNPoly : ZoneForm
    {
        public int[] _x;
        public int[] _y;
        public int _z1;
        public int _z2;

        public int minX = int.MaxValue,
                   maxX,
                   minY = int.MaxValue,
                   maxY;

        public ZoneNPoly(int[] x, int[] y, int z1, int z2)
        {
            _x = x;
            _y = y;
            _z1 = z1;
            _z2 = z2;

            foreach (int a in x)
            {
                if (a > maxX)
                    maxX = a;

                if (a < minX)
                    minX = a;
            }

            foreach (int a in y)
            {
                if (a > maxY)
                    maxY = a;

                if (a < minY)
                    minY = a;
            }
        }

        public override bool isInsideZone(int x, int y, int z)
        {
            if ((z < _z1) || (z > _z2))
                return false;

            bool inside = false;
            for (int i = 0,
                     j = _x.Length - 1; i < _x.Length; j = i++)
                if ((((_y[i] <= y) && (y < _y[j])) || ((_y[j] <= y) && (y < _y[i]))) && (x < ((((_x[j] - _x[i]) * (y - _y[i])) / (_y[j] - _y[i])) + _x[i])))
                    inside = !inside;

            return inside;
        }

        public bool isInsideZone(int x, int y)
        {
            bool inside = false;
            for (int i = 0,
                     j = _x.Length - 1; i < _x.Length; j = i++)
                if ((((_y[i] <= y) && (y < _y[j])) || ((_y[j] <= y) && (y < _y[i]))) && (x < ((((_x[j] - _x[i]) * (y - _y[i])) / (_y[j] - _y[i])) + _x[i])))
                    inside = !inside;

            return inside;
        }

        public override double getDistanceToZone(int x, int y)
        {
            double shortestDist = Math.Pow(_x[0] - x, 2) + Math.Pow(_y[0] - y, 2);

            for (int i = 1; i < _y.Length; i++)
            {
                double test = Math.Pow(_x[i] - x, 2) + Math.Pow(_y[i] - y, 2);
                if (test < shortestDist)
                    shortestDist = test;
            }

            return Math.Sqrt(shortestDist);
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