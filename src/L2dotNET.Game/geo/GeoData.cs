using L2dotNET.GameService.Properties;
using L2dotNET.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GameService.geo
{
    /// <summary>
    /// Geodata engine.
    /// </summary>
    internal static class Geodata
    {
        private const string m_GeoFileMask = "*.l2j";
        private const int m_MinGeoFileLength = 0x30000;

        private static readonly ILog log = LogManager.GetLogger(typeof(Geodata));

        private static readonly SortedList<short, GeoBlock[]> geodata = new SortedList<short, GeoBlock[]>();

        /// <summary>
        /// Initializes <see cref="Geodata"/> engine.
        /// </summary>
        /// <returns>True, if <see cref="Geodata"/> engine was initialized successfully, otherwise false.</returns>
        internal static unsafe bool Initialize()
        {
            //return true;

            try
            {
                FileInfo[] filesInfoArray = L2FileReader.GetFiles
                    (
                        Path.Combine
                            (
                                AppDomain.CurrentDomain.BaseDirectory,
                                @"./geo/"
                            ),
                        m_GeoFileMask,
                        SearchOption.TopDirectoryOnly
                    );

                FileInfo currentFile;

                int counter, reader, i, j, k, m;
                short[] heightsComplex = new short[0x40], heightsMultilayered = new short[0x900];
                byte[] heightsMap = new byte[0x40];
                ushort[] offsetsMap = new ushort[0x40];
                GeoBlock[] nextMap;
#if GEO_FIX_DATA
                bool flat;
#endif
                for (i = 0; i < filesInfoArray.Length; i++)
                {
                    currentFile = filesInfoArray[i];

                    log.Info($"Loading {currentFile.Name} ");

                    counter = reader = 0;
                    nextMap = new GeoBlock[ushort.MaxValue];

                    fixed (byte* buffer = L2FileReader.Read(currentFile.FullName, (int)currentFile.Length))
                    {
                        while (counter < ushort.MaxValue)
                        {
                            switch (*(buffer + reader++))
                            {
                                case 0x00:
                                    {
                                        nextMap[counter] = new GeoBlock(null, null, *(short*)(buffer + reader));
                                        reader += sizeof(short);
                                        break;
                                    }
                                case 0x01:
                                    {
#if GEO_FIX_DATA
                                        flat = true;
#endif
                                        fixed (short* heights = heightsComplex)
                                        {
                                            for (j = 0; j < 0x40; reader += sizeof(short), j++)
                                            {
                                                *(heights + j) = (short)(*(short*)(buffer + reader) >> 1);

#if GEO_FIX_DATA
                                                if ( j > 0 && *( heights + j ) != *( heights + j - 1 ) )
                                                    flat = false; // validating that not all heights are same
#endif
                                            }
                                        }
#if GEO_FIX_DATA
                                        if ( flat )
                                            nextMap[counter] = new GeoBlock(null, null, heightsComplex[0]);
                                        else
#endif
                                        nextMap[counter] = new GeoBlock(null, null, heightsComplex);

                                        break;
                                    }
                                case 0x02:
                                    {
                                        fixed (ushort* offsets = offsetsMap)
                                        fixed (byte* map = heightsMap)
                                        fixed (short* heights = heightsMultilayered)
                                        {
                                            for (j = 0, m = 0; j < 0x40; j++)
                                            {
                                                *(map + j) = *(byte*)(buffer + reader++);

                                                for (k = 0; k < *(map + j); k++, m++)
                                                {
                                                    *(heights + m) = (short)(*(short*)(buffer + reader) >> 1);
                                                    reader += sizeof(short);
                                                }

                                                *(offsets + j) = (ushort)(k + *(offsets + j - 0x01));
                                            }

#if GEO_FIX_DATA
                                            if ( offsetsMap[0x3f] == 0x40 ) // only 64 heights, so block is complex or flat
                                            {
                                                flat = true;

                                                for ( j = 1; j < 0x40; j++ )
                                                {
                                                    if ( offsetsMap[j] != offsetsMap[j - 1] )
                                                    {
                                                        flat = false; // validating that not all heights are same
                                                        break;
                                                    }
                                                }

                                                if ( flat )
                                                    nextMap[counter] = new GeoBlock(null, null, heights[0]);
                                                else
                                                    nextMap[counter] = new GeoBlock(null, null, L2Buffer.SpecialCopy(heights, 0x40));
                                            }
                                            else
#endif
                                            nextMap[counter] = new GeoBlock(heightsMap, offsetsMap, L2Buffer.SpecialCopy(heights, offsetsMap[0x3f]));
                                        }

                                        break;
                                    }
                                default:
                                    throw new InvalidOperationException(String.Format("Failed to read geodata file '{0}'", currentFile.FullName));
                            }

                            counter++;

                           // if (counter % (ushort.MaxValue / 20) == 0)
                           //     log.Info(".");
                        }
                    }

                    geodata.Add(ParseMapOffsets(currentFile), nextMap);
                }

                geodata.TrimExcess();

                filesInfoArray = null;
                currentFile = null;
                heightsComplex = null;
                heightsMultilayered = null;
                heightsMap = null;
                offsetsMap = null;
                nextMap = null;

                GC.WaitForPendingFinalizers();
                GC.Collect();

                return true;
            }
            catch (Exception e)
            {
                log.Error("Failed to load geodata files. Error stack trace: " + e);
            }

            return false;
        }

        /// <summary>
        /// Parses map absolute offset for provided <see cref="FileInfo"/> object ( for now file name must be X_Y.* ).
        /// </summary>
        /// <param name="fileInfo"><see cref="FileInfo"/> for file to parse it's absolute offset.</param>
        /// <returns>Parsed absolute map offset.</returns>
        private static short ParseMapOffsets(FileInfo fileInfo)
        {
            string[] tmp = fileInfo.Name.Replace(fileInfo.Extension, String.Empty).Split('_');

            if (tmp.Length != 0x02)
                throw new ArgumentException(String.Format("Failed to parse geodata file offsets, file name: '{0}'", fileInfo.Name));

            byte[] offsets = new byte[0x02];

            if (!byte.TryParse(tmp[0], out offsets[0]) || !byte.TryParse(tmp[1], out offsets[1]))
                throw new ArgumentException(String.Format("Failed to parse geodata file offsets, file name: '{0}'", fileInfo.Name));

            return (short)((offsets[0x00] << 0x05) + offsets[0x01]);
        }

        private static int Abs(int v)
        {
            return v > 0 ? v : -v;
        }
    }
}
