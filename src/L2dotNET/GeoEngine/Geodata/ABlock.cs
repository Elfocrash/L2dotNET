using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GeoEngine
{
    public abstract class ABlock
    {
        public abstract bool HasGeoPos();

        public abstract short GetHeightNearest(int geoX, int geoY, int worldZ);

        public abstract short GetHeightNearestOriginal(int geoX, int geoY, int worldZ);

        public abstract short GetHeightAbove(int geoX, int geoY, int worldZ);

        public abstract short GetHeightBelow(int geoX, int geoY, int worldZ);

        public abstract byte GetNsweNearest(int geoX, int geoY, int worldZ);

        public abstract byte GetNsweNearestOriginal(int geoX, int geoY, int worldZ);

        public abstract byte GetNsweAbove(int geoX, int geoY, int worldZ);

        public abstract byte GetNsweBelow(int geoX, int geoY, int worldZ);

        public abstract int GetIndexNearest(int geoX, int geoY, int worldZ);

        public abstract int GetIndexAbove(int geoX, int geoY, int worldZ);

        public abstract int GetIndexAboveOriginal(int geoX, int geoY, int worldZ);

        public abstract int GetIndexBelow(int geoX, int geoY, int worldZ);

        public abstract int GetIndexBelowOriginal(int geoX, int geoY, int worldZ);

        public abstract short GetHeight(int index);

        public abstract short GetHeightOriginal(int index);

        public abstract byte GetNswe(int index);

        public abstract byte GetNsweOriginal(int index);

        public abstract void SetNswe(int index, byte nswe);

        public abstract void SaveBlock(BufferedStream stream);
    }
}
