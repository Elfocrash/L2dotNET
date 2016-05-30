using L2dotNET.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GameService.model.zones.forms
{
    public abstract class L2ZoneForm
    {
        protected static int STEP = 20;

        public abstract bool IsInsideZone(int x, int y, int z);

        public abstract bool IntersectsRectangle(int x1, int x2, int y1, int y2);

        public abstract double GetDistanceToZone(int x, int y);

        public abstract int GetLowZ();

        public abstract int GetHighZ();

        protected bool LineSegmentsIntersect(int ax1, int ay1, int ax2, int ay2, int bx1, int by1, int bx2, int by2)
        {
            return MathHelper.LineIntersect(ax1, ay1, ax2, ay2, bx1, by1, bx2, by2);
        }

        public abstract void VisualizeZone(int z);
    }
}
