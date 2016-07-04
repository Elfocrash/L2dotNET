using System;
using System.IO;

namespace L2dotNET.GeoEngine.Geodata
{
    public class BlockMultilayer : ABlock
    {
        private const int MaxLayers = byte.MaxValue;

        private static MemoryStream _temp;

        protected byte[] Buffer;

        public static void Initialize()
        {
            _temp = new MemoryStream(GeoStructure.BlockCells * MaxLayers * 3);

            if (!BitConverter.IsLittleEndian)
            {
                byte[] superTemp = _temp.ToArray();
                Array.Reverse(superTemp);
                _temp.Write(superTemp, 0, _temp.ToArray().Length);
            }
        }

        public static void Release()
        {
            _temp = null;
        }

        protected BlockMultilayer()
        {
            Buffer = null;
        }

        public BlockMultilayer(MemoryStream ms)
        {
            for (int cell = 0; cell < GeoStructure.BlockCells; cell++)
            {
                byte layers = (byte)ms.ReadByte();

                _temp.Write(ms.ToArray(), 1, layers);

                // loop over layers
                for (byte layer = 0; layer < layers; layer++)
                {
                    // add nswe
                    _temp.WriteByte(ms.ToArray()[layer]);

                    // add height
                    _temp.WriteByte(ms.ToArray()[layer]);
                }
            }

            Array.Copy(_temp.ToArray(), Buffer, _temp.Position);
            _temp.SetLength(0);
        }

        public override short GetHeight(int index)
        {
            return (short)((Buffer[index + 1] & 0x00FF) | (Buffer[index + 2] << 8));
        }

        public override short GetHeightAbove(int geoX, int geoY, int worldZ)
        {
            int index = 0;
            for (int i = 0; i < (geoX % GeoStructure.BlockCellsX) * GeoStructure.BlockCellsY + (geoY % GeoStructure.BlockCellsY); i++)
                index += Buffer[index] * 3 + 1;

            byte layers = Buffer[index++];
            index += (layers - 1) * 3;

            while (layers-- > 0)
            {
                int height = (Buffer[index + 1] & 0x00FF) | (Buffer[index + 2] << 8);

                if (height > worldZ)
                    return (short)height;

                index -= 3;
            }

            return short.MinValue;
        }

        public override short GetHeightBelow(int geoX, int geoY, int worldZ)
        {
            int index = 0;
            for (int i = 0; i < (geoX % GeoStructure.BlockCellsX) * GeoStructure.BlockCellsY + (geoY % GeoStructure.BlockCellsY); i++)
                index += Buffer[index] * 3 + 1;

            byte layers = Buffer[index++];

            while (layers-- > 0)
            {
                int height = (Buffer[index + 1] & 0x00FF) | (Buffer[index + 2] << 8);

                if (height < worldZ)
                    return (short)height;

                index += 3;
            }

            return short.MaxValue;
        }

        public override short GetHeightNearest(int geoX, int geoY, int worldZ)
        {
            int index = GetIndexNearest(geoX, geoY, worldZ);

            return (short)((Buffer[index + 1] & 0x00FF) | (Buffer[index + 2] << 8));
        }

        public override short GetHeightNearestOriginal(int geoX, int geoY, int worldZ)
        {
            return GetHeightNearest(geoX, geoY, worldZ);
        }

        public override short GetHeightOriginal(int index)
        {
            return (short)((Buffer[index + 1] & 0x00FF) | (Buffer[index + 2] << 8));
        }

        public override int GetIndexAbove(int geoX, int geoY, int worldZ)
        {
            int index = 0;
            for (int i = 0; i < (geoX % GeoStructure.BlockCellsX) * GeoStructure.BlockCellsY + (geoY % GeoStructure.BlockCellsY); i++)
                index += Buffer[index] * 3 + 1;

            byte layers = Buffer[index++];
            index += (layers - 1) * 3;

            while (layers-- > 0)
            {
                int height = (Buffer[index + 1] & 0x00FF) | (Buffer[index + 2] << 8);

                if (height > worldZ)
                    return index;

                index -= 3;
            }

            return -1;
        }

        public override int GetIndexAboveOriginal(int geoX, int geoY, int worldZ)
        {
            return GetIndexAbove(geoX, geoY, worldZ);
        }

        public override int GetIndexBelow(int geoX, int geoY, int worldZ)
        {
            int index = 0;
            for (int i = 0; i < (geoX % GeoStructure.BlockCellsX) * GeoStructure.BlockCellsY + (geoY % GeoStructure.BlockCellsY); i++)
                index += Buffer[index] * 3 + 1;

            byte layers = Buffer[index++];

            while (layers-- > 0)
            {
                int height = (Buffer[index + 1] & 0x00FF) | (Buffer[index + 2] << 8);

                if (height < worldZ)
                    return index;

                index += 3;
            }

            return -1;
        }

        public override int GetIndexBelowOriginal(int geoX, int geoY, int worldZ)
        {
            return GetIndexBelow(geoX, geoY, worldZ);
        }

        public override int GetIndexNearest(int geoX, int geoY, int worldZ)
        {
            int index = 0;
            for (int i = 0; i < (geoX % GeoStructure.BlockCellsX) * GeoStructure.BlockCellsY + (geoY % GeoStructure.BlockCellsY); i++)
                index += Buffer[index] * 3 + 1;

            byte layers = Buffer[index++];

            int limit = int.MaxValue;
            while (layers-- > 0)
            {
                int height = (Buffer[index + 1] & 0x00FF) | (Buffer[index + 2] << 8);

                int distance = Math.Abs(height - worldZ);
                if (distance > limit)
                    break;

                limit = distance;
                index += 3;
            }

            return index - 3;
        }

        public override byte GetNswe(int index)
        {
            return Buffer[index];
        }

        public override byte GetNsweAbove(int geoX, int geoY, int worldZ)
        {
            int index = 0;
            for (int i = 0; i < (geoX % GeoStructure.BlockCellsX) * GeoStructure.BlockCellsY + (geoY % GeoStructure.BlockCellsY); i++)
                index += Buffer[index] * 3 + 1;

            byte layers = Buffer[index++];
            index += (layers - 1) * 3;

            while (layers-- > 0)
            {
                int height = (Buffer[index + 1] & 0x00FF) | (Buffer[index + 2] << 8);

                if (height > worldZ)
                    return Buffer[index];

                index -= 3;
            }

            return 0;
        }

        public override byte GetNsweBelow(int geoX, int geoY, int worldZ)
        {
            int index = 0;
            for (int i = 0; i < (geoX % GeoStructure.BlockCellsX) * GeoStructure.BlockCellsY + (geoY % GeoStructure.BlockCellsY); i++)
                index += Buffer[index] * 3 + 1;

            byte layers = Buffer[index++];

            while (layers-- > 0)
            {
                int height = (Buffer[index + 1] & 0x00FF) | (Buffer[index + 2] << 8);

                if (height < worldZ)
                    return Buffer[index];

                index += 3;
            }

            return 0;
        }

        public override byte GetNsweNearest(int geoX, int geoY, int worldZ)
        {
            int index = GetIndexNearest(geoX, geoY, worldZ);

            return Buffer[index];
        }

        public override byte GetNsweNearestOriginal(int geoX, int geoY, int worldZ)
        {
            return GetNsweNearest(geoX, geoY, worldZ);
        }

        public override byte GetNsweOriginal(int index)
        {
            return Buffer[index];
        }

        public override bool HasGeoPos()
        {
            return true;
        }

        public override void SaveBlock(BufferedStream stream)
        {
            stream.WriteByte(GeoStructure.TypeMultilayerL2D);

            int index = 0;
            for (int i = 0; i < GeoStructure.BlockCells; i++)
            {
                byte layers = Buffer[index++];
                stream.WriteByte(layers);

                stream.Write(Buffer, index, layers * 3);

                index += layers * 3;
            }
        }

        public override void SetNswe(int index, byte nswe)
        {
            Buffer[index] = nswe;
        }
    }
}