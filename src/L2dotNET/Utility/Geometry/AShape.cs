using L2dotNET.DataContracts;
using L2dotNET.Models;
using L2dotNET.Models.zones;

namespace L2dotNET.Utility.Geometry
{
    public abstract class AShape
    {
        public abstract int GetSize();

        public abstract double GetArea();

        public abstract double GetVolume();

        public abstract bool IsInside(int x, int y);

        public abstract bool IsInside(int x, int y, int z);

        public abstract Location GetRandomLocation();
    }
}