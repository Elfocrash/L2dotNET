using System.IO;

namespace L2dotNET.GeoEngine.Geodata
{
    public class BlockNull : ABlock
    {
        private readonly byte _nswe;

        public BlockNull()
        {
            _nswe = (byte)0xFF;
        }

        public override short GetHeight(int index)
        {
            return 0;
        }

        public override short GetHeightAbove(int geoX, int geoY, int worldZ)
        {
            return (short)worldZ;
        }

        public override short GetHeightBelow(int geoX, int geoY, int worldZ)
        {
            return (short)worldZ;
        }

        public override short GetHeightNearest(int geoX, int geoY, int worldZ)
        {
            return (short)worldZ;
        }

        public override short GetHeightNearestOriginal(int geoX, int geoY, int worldZ)
        {
            return (short)worldZ;
        }

        public override short GetHeightOriginal(int index)
        {
            return 0;
        }

        public override int GetIndexAbove(int geoX, int geoY, int worldZ)
        {
            return 0;
        }

        public override int GetIndexAboveOriginal(int geoX, int geoY, int worldZ)
        {
            return 0;
        }

        public override int GetIndexBelow(int geoX, int geoY, int worldZ)
        {
            return 0;
        }

        public override int GetIndexBelowOriginal(int geoX, int geoY, int worldZ)
        {
            return 0;
        }

        public override int GetIndexNearest(int geoX, int geoY, int worldZ)
        {
            return 0;
        }

        public override byte GetNswe(int index)
        {
            return _nswe;
        }

        public override byte GetNsweAbove(int geoX, int geoY, int worldZ)
        {
            return _nswe;
        }

        public override byte GetNsweBelow(int geoX, int geoY, int worldZ)
        {
            return _nswe;
        }

        public override byte GetNsweNearest(int geoX, int geoY, int worldZ)
        {
            return _nswe;
        }

        public override byte GetNsweNearestOriginal(int geoX, int geoY, int worldZ)
        {
            return _nswe;
        }

        public override byte GetNsweOriginal(int index)
        {
            return _nswe;
        }

        public override bool HasGeoPos()
        {
            return false;
        }

        public override void SaveBlock(BufferedStream stream) { }

        public override void SetNswe(int index, byte nswe) { }
    }
}