using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2dotNET.Game.geo
{
    public class Location
    {
        protected int x;
        protected int y;
        protected int z;
        public int h = 0;
        public int r = 0;

        public Location()
        {
            x = 0;
            y = 0;
            z = 0;
            h = 0;
        }

        public Location(int[] point)
        {
            h = 0;
            x = point[0];
            y = point[1];
            z = point[2];
            h = point[3];
        }

        public Location(int locX, int locY, int locZ)
        {
            x = locX;
            y = locY;
            z = locZ;
        }

        public Location(int locX, int locY, int locZ, int heading)
        {
            x = locX;
            y = locY;
            z = locZ;
            h = heading;
        }

        public bool equals(Location loc)
        {
            return loc.getX() == x && loc.getY() == y && loc.getZ() == z;
        }

        public bool equals(int _x, int _y, int _z)
        {
            return _x == x && _y == y && _z == z;
        }

        public Location changeZ(int zDiff)
        {
            z += zDiff;
            return this;
        }

        public Location setX(int x)
        {
            this.x = x;
            return this;
        }

        public Location setY(int y)
        {
            this.y = y;
            return this;
        }

        public Location setZ(int z)
        {
            this.z = z;
            return this;
        }

        public Location setH(int h)
        {
            this.h = h;
            return this;
        }

        public Location setR(int r)
        {
            this.r = r;
            return this;
        }

        public void set(Location l)
        {
            x = l.getX();
            y = l.getY();
            z = l.getZ();
        }

        public void set(int _x, int _y, int _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public void set(int _x, int _y, int _z, int _h)
        {
            x = _x;
            y = _y;
            z = _z;
            h = _h;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getZ()
        {
            return z;
        }

        public int getReflect()
        {
            return r;
        }

        public int getHeading()
        {
            return h;
        }

        public String toStringS()
        {
            return "Coords(" + x + "," + y + "," + z + "," + h + ")";
        }

        public double distance(Location loc)
        {
            return distance(loc.x, loc.y);
        }

        public double distance(int _x, int _y)
        {
            int dx = x - _x;
            int dy = y - _y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public String toXYZString()
        {
            return x + "," + y + "," + z;
        }

        public Location geo2world()
        {
            // размер одного блока 16*16 точек, +8*+8 это его средина
            x = (x << 4) + GeoData.MAP_MIN_X + 8;
            y = (y << 4) + GeoData.MAP_MIN_Y + 8;
            return this;
        }
    }
}
