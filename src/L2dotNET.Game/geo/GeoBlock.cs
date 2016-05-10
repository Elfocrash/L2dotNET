using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Game.geo
{
    /// <summary>
    /// Represents <see cref="GeoBlock"/> types.
    /// </summary>
    internal enum GeoBlockType : byte
    {
        /// <summary>
        /// Flat <see cref="GeoBlock"/> type.
        /// </summary>
        Flat = 0x00,
        /// <summary>
        /// Complex <see cref="GeoBlock"/> type.
        /// </summary>
        Complex = 0x01,
        /// <summary>
        /// Multi layered <see cref="GeoBlock"/> type.
        /// </summary>
        MultiLayered = 0x02
    }

    /// <summary>
    /// Represents geodata block object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct GeoBlock
    //internal sealed class GeoBlock
    {
        /// <summary>
        /// Block heights map.
        /// </summary>
        private readonly byte[] m_HeightsMap;

        /// <summary>
        /// Block height offsets.
        /// </summary>
        private readonly ushort[] m_HeightOffsets;

        /// <summary>
        /// Height values.
        /// </summary>
        private readonly short[] m_Heights;

        /// <summary>
        /// Gets current <see cref="GeoBlock"/> object <see cref="GeoBlockType"/>.
        /// </summary>
        public readonly GeoBlockType Type;

        /// <summary>
        /// Initializes new instance of <see cref="GeoBlock"/> object.
        /// </summary>
        /// <param name="heightsMap">m_Heights map.</param>
        /// <param name="heigthOffsets">Height offsets.</param>
        /// <param name="heights">Height values.</param>
        public GeoBlock(byte[] heightsMap, ushort[] heigthOffsets, params short[] heights)
        {
            m_HeightsMap = heightsMap;
            m_Heights = heights;
            m_HeightOffsets = heigthOffsets;

            switch (m_Heights.Length)
            {
                case 0x01:
                    Type = GeoBlockType.Flat;
                    return;
                case 0x40:
                    Type = GeoBlockType.Complex;
                    return;
                default:
                    Type = GeoBlockType.MultiLayered;
                    return;
            }
        }

        public unsafe short[] GetHeights(int cellOffset)
        {
            if (m_HeightsMap == null)
                return m_Heights;

            int layersCount = m_HeightsMap[cellOffset];
            ushort offset = m_HeightOffsets[cellOffset];

            short[] heights = new short[layersCount];

            layersCount = 0;

            fixed (short* h = heights, src = m_Heights)
                while (layersCount < heights.Length)
                *(h + layersCount++) = *(src + offset++);

            return heights;
        }
    }
}
