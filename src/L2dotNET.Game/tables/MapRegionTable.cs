using System.Data;
using L2dotNET.Game.logger;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IO;
using System;

namespace L2dotNET.Game.tables
{
    class MapRegionTable
    {
        private static MapRegionTable instance = new MapRegionTable();
        public static MapRegionTable getInstance()
        {
            return instance;
        }

        private byte[][] RegionMap;
        private byte shiftX = 16, shiftY = 18;
        public SortedList<short, RegionID> _regions;
        public SortedList<short, ZoneRespawn> RegionRespawns;
        public MapRegionTable()
        {
            RegionMap = new byte[shiftX][];
            for (byte a = 0; a < shiftX; a++)
                RegionMap[a] = new byte[shiftY];

            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = "SELECT * FROM world_mapregion";
            //cmd.CommandType = CommandType.Text;

            //MySqlDataReader reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    short region = reader.GetInt16("region");

            //    for (byte a = 0; a < shiftX; a++)
            //    {
            //        short regId = reader.GetInt16("sec" + (a + 11));
            //        RegionMap[a][region - 8] = (byte)regId;
            //    }
            //}

            //reader.Close();

            //_regions = new SortedList<short, RegionID>();
            //cmd = connection.CreateCommand();

            //cmd.CommandText = "SELECT * FROM world_mapid";
            //cmd.CommandType = CommandType.Text;

            //reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    RegionID reg = new RegionID();
            //    reg.id = reader.GetInt16("regionId");
            //    reg.name = reader.GetString("name");
            //    reg.sysId = reader.GetInt32("sysId");
            //    reg.castleId = reader.GetInt16("districtId");
            //    reg.bbsId = reader.GetInt16("bbs");
            //    reg.townId = reader.GetInt16("townId");

            //    _regions.Add(reg.id, reg);
            //}

            //reader.Close();
            //connection.Close();

            RegionRespawns = new SortedList<short, ZoneRespawn>();
            StreamReader sr = new StreamReader(new FileInfo(@"scripts\zone_respawn.txt").FullName);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line.Length == 0 || line.StartsWith("#"))
                    continue;

                string[] pt = line.Split('\t');

                ZoneRespawn resp = new ZoneRespawn();
                resp.RegionID = Convert.ToInt16(pt[0]);

                for (byte ord = 1; ord < pt.Length; ord++)
                {
                    string parameter = pt[ord];
                    string value = parameter.Substring(parameter.IndexOf('{') + 1); value = value.Remove(value.Length - 1);

                    switch (parameter.Split('{')[0].ToLower())
                    {
                        case "point":
                            foreach (string str in value.Split(';'))
                                resp.normal(str.Split(' '));
                            break;
                        case "chao_point":
                            foreach (string str in value.Split(';'))
                                resp.karma(str.Split(' '));
                            break;
                    }
                }

                resp.end();
                RegionRespawns.Add(resp.RegionID, resp);
            }
            sr.Close();

            CLogger.info("MapRegion: loaded " + (shiftX * shiftY) + " regions with " + _regions.Count + " ids, resps " + RegionRespawns.Count);
        }

        private int mapShiftX(int x)
        {
            return (x >> 15) + 9;
        }

        private int mapShiftY(int y)
        {
            return (y >> 15) + 10;
        }

        public RegionID getRegionId(int x, int y)
        {
            short region = RegionMap[mapShiftX(x)][mapShiftY(y)];

            if (_regions.ContainsKey(region))
                return _regions[region];
            return null;
        }

        public int getRegionSysId(int x, int y)
        {
            RegionID reg = this.getRegionId(x, y);
            return reg == null ? -1 : reg.sysId;
        }

        public int getRegionCastleId(int x, int y)
        {
            RegionID reg = this.getRegionId(x, y);
            return reg == null ? 5 : reg.castleId;
        }

        public int getRegionBBSId(int x, int y)
        {
            RegionID reg = this.getRegionId(x, y);
            return reg == null ? -1 : reg.bbsId;
        }

        public int getRegionTownId(int x, int y)
        {
            RegionID reg = this.getRegionId(x, y);
            return reg == null ? 10 : reg.townId;
        }

        public int[] getRespawn(int x, int y, int karma)
        {
            short townId = (short)getRegionTownId(x, y);

            if (!RegionRespawns.ContainsKey(townId))
            {
                townId = 10; // если в жопе то переносим в аден
                CLogger.error("MapRegion.getRespawn for " + x + " " + y + " (" + townId + ") not found.");
            }

            ZoneRespawn zr = RegionRespawns[townId];
            Random rn = new Random();
            if (karma > 0)
            {
                if(zr.karmaLocArray == null)
                    return zr.normalLocArray[rn.Next(zr.normalLocArray.Length - 1)]; 
                else
                    return zr.karmaLocArray[rn.Next(zr.karmaLocArray.Length - 1)]; 
            }
            else
                return zr.normalLocArray[rn.Next(zr.normalLocArray.Length - 1)]; 
        }
    }

    public class ZoneRespawn
    {
        public short RegionID;

        public int[][] normalLocArray;
        List<int[]> normalLoc;
        public void normal(string[] p)
        {
            if (normalLoc == null)
                normalLoc = new List<int[]>();

            normalLoc.Add(new int[] { Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]) });
        }

        public int[][] karmaLocArray;
        List<int[]> karmalLoc;
        public void karma(string[] p)
        {
            if (karmalLoc == null)
                karmalLoc = new List<int[]>();

            karmalLoc.Add(new int[] { Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]) });
        }

        public void end()
        {
            normalLocArray = normalLoc.ToArray();
            normalLoc.Clear();
            karmaLocArray = karmalLoc.ToArray();
            karmalLoc.Clear();
        }
    }
}
