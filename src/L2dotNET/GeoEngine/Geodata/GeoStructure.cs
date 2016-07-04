using System;

namespace L2dotNET.GeoEngine.Geodata
{
    public class GeoStructure
    {
        public static int TileXMin = 16;
        public static int TileXMax = 26;
        public static int TileYMin = 10;
        public static int TileYMax = 25;

        public static int TileSize = 32768;
        public static int WorldXMin = (TileXMin - 20) * TileSize;
        public static int WorldXMax = (TileXMax - 19) * TileSize;
        public static int WorldYMin = (TileYMin - 18) * TileSize;
        public static int WorldYMax = (TileYMax - 17) * TileSize;
        private const int RegionSize = 4096;
        private static int _regionsX = (WorldXMax - WorldXMin) / RegionSize;
        private static int _regionsY = (WorldYMax - WorldYMin) / RegionSize;
        private static int _regionXOffset = Math.Abs(WorldXMin / RegionSize);
        private static int _regionYOffset = Math.Abs(WorldYMin / RegionSize);
        public static readonly byte CellFlagE = 1 << 0;
        public static readonly byte CellFlagW = 1 << 1;
        public static readonly byte CellFlagS = 1 << 2;
        public static readonly byte CellFlagN = 1 << 3;
        public static readonly byte CellFlagSe = 1 << 4;
        public static readonly byte CellFlagSw = 1 << 5;
        public static readonly byte CellFlagNe = 1 << 6;
        public static readonly byte CellFlagNw = 1 << 7;
        public static readonly byte CellFlagSAndE = (byte)(CellFlagS | CellFlagE);
        public static readonly byte CellFlagSAndW = (byte)(CellFlagS | CellFlagW);
        public static readonly byte CellFlagNAndE = (byte)(CellFlagN | CellFlagE);
        public static readonly byte CellFlagNAndW = (byte)(CellFlagN | CellFlagW);

        public static readonly int CellSize = 16;
        public static readonly int CellHeight = 8;
        public static readonly int CellIgnoreHeight = CellHeight * 6;

        public static readonly byte TypeFlatL2Jl2Off = 0;
        public static readonly byte TypeFlatL2D = 0xD0;
        public static readonly byte TypeComplexL2J = 1;
        public static readonly byte TypeComplexL2Off = 0x40;
        public static readonly byte TypeComplexL2D = 0xD1;
        public static readonly byte TypeMultilayerL2J = 2;
        public static readonly byte TypeMultilayerL2D = 0xD2;

        public static readonly int BlockCellsX = 8;
        public static readonly int BlockCellsY = 8;
        public static readonly int BlockCells = BlockCellsX * BlockCellsY;

        public static readonly int RegionBlocksX = 256;
        public static readonly int RegionBlocksY = 256;
        public static readonly int RegionBlocks = RegionBlocksX * RegionBlocksY;

        public static readonly int RegionCellsX = RegionBlocksX * BlockCellsX;
        public static readonly int RegionCellsY = RegionBlocksY * BlockCellsY;

        public static readonly int GeoRegionsX = (TileXMax - TileXMin + 1);
        public static readonly int GeoRegionsY = (TileYMax - TileYMin + 1);

        public static readonly int GeoBlocksX = GeoRegionsX * RegionBlocksX;
        public static readonly int GeoBlocksY = GeoRegionsY * RegionBlocksY;

        public static readonly int GeoCellsX = GeoBlocksX * BlockCellsX;
        public static readonly int GeoCellsY = GeoBlocksY * BlockCellsY;
    }
}