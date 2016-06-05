using System;

namespace L2dotNET.GeoEngine.Geodata
{
    public class GeoStructure
    {
        public static int TILE_X_MIN = 16;
        public static int TILE_X_MAX = 26;
        public static int TILE_Y_MIN = 10;
        public static int TILE_Y_MAX = 25;

        public static int TILE_SIZE = 32768;
        public static int WORLD_X_MIN = (TILE_X_MIN - 20) * TILE_SIZE;
        public static int WORLD_X_MAX = (TILE_X_MAX - 19) * TILE_SIZE;
        public static int WORLD_Y_MIN = (TILE_Y_MIN - 18) * TILE_SIZE;
        public static int WORLD_Y_MAX = (TILE_Y_MAX - 17) * TILE_SIZE;
        private static readonly int REGION_SIZE = 4096;
        private static int REGIONS_X = (WORLD_X_MAX - WORLD_X_MIN) / REGION_SIZE;
        private static int REGIONS_Y = (WORLD_Y_MAX - WORLD_Y_MIN) / REGION_SIZE;
        private static int REGION_X_OFFSET = Math.Abs(WORLD_X_MIN / REGION_SIZE);
        private static int REGION_Y_OFFSET = Math.Abs(WORLD_Y_MIN / REGION_SIZE);
        public static readonly byte CELL_FLAG_E = 1 << 0;
        public static readonly byte CELL_FLAG_W = 1 << 1;
        public static readonly byte CELL_FLAG_S = 1 << 2;
        public static readonly byte CELL_FLAG_N = 1 << 3;
        public static readonly byte CELL_FLAG_SE = 1 << 4;
        public static readonly byte CELL_FLAG_SW = 1 << 5;
        public static readonly byte CELL_FLAG_NE = 1 << 6;
        public static readonly byte CELL_FLAG_NW = (byte)(1 << 7);
        public static readonly byte CELL_FLAG_S_AND_E = (byte)(CELL_FLAG_S | CELL_FLAG_E);
        public static readonly byte CELL_FLAG_S_AND_W = (byte)(CELL_FLAG_S | CELL_FLAG_W);
        public static readonly byte CELL_FLAG_N_AND_E = (byte)(CELL_FLAG_N | CELL_FLAG_E);
        public static readonly byte CELL_FLAG_N_AND_W = (byte)(CELL_FLAG_N | CELL_FLAG_W);

        public static readonly int CELL_SIZE = 16;
        public static readonly int CELL_HEIGHT = 8;
        public static readonly int CELL_IGNORE_HEIGHT = CELL_HEIGHT * 6;

        public static readonly byte TYPE_FLAT_L2J_L2OFF = 0;
        public static readonly byte TYPE_FLAT_L2D = (byte)0xD0;
        public static readonly byte TYPE_COMPLEX_L2J = 1;
        public static readonly byte TYPE_COMPLEX_L2OFF = 0x40;
        public static readonly byte TYPE_COMPLEX_L2D = (byte)0xD1;
        public static readonly byte TYPE_MULTILAYER_L2J = 2;
        public static readonly byte TYPE_MULTILAYER_L2D = (byte)0xD2;

        public static readonly int BLOCK_CELLS_X = 8;
        public static readonly int BLOCK_CELLS_Y = 8;
        public static readonly int BLOCK_CELLS = BLOCK_CELLS_X * BLOCK_CELLS_Y;

        public static readonly int REGION_BLOCKS_X = 256;
        public static readonly int REGION_BLOCKS_Y = 256;
        public static readonly int REGION_BLOCKS = REGION_BLOCKS_X * REGION_BLOCKS_Y;

        public static readonly int REGION_CELLS_X = REGION_BLOCKS_X * BLOCK_CELLS_X;
        public static readonly int REGION_CELLS_Y = REGION_BLOCKS_Y * BLOCK_CELLS_Y;

        public static readonly int GEO_REGIONS_X = (TILE_X_MAX - TILE_X_MIN + 1);
        public static readonly int GEO_REGIONS_Y = (TILE_Y_MAX - TILE_Y_MIN + 1);

        public static readonly int GEO_BLOCKS_X = GEO_REGIONS_X * REGION_BLOCKS_X;
        public static readonly int GEO_BLOCKS_Y = GEO_REGIONS_Y * REGION_BLOCKS_Y;

        public static readonly int GEO_CELLS_X = GEO_BLOCKS_X * BLOCK_CELLS_X;
        public static readonly int GEO_CELLS_Y = GEO_BLOCKS_Y * BLOCK_CELLS_Y;
    }
}