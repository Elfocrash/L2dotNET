using System;
using System.Collections.Generic;
using L2dotNET.Models.Zones.forms;
using L2dotNET.Utility;
using NLog;

namespace L2dotNET.Tables
{
    public class L2Territory
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public string Name;
        public string Controller;
        public bool StartActive;
        public List<L2Spawn> Spawns = new List<L2Spawn>();
        public List<int[]> TerritoryLoc = new List<int[]>();
        public ZoneNPoly Territory;

        public void AddNpc(int id, int count, string respawn, string pos)
        {
            int value = Convert.ToInt32(respawn.Remove(respawn.Length - 1));

            if (respawn.Contains("s"))
                value *= 1000;
            else
            {
                if (respawn.Contains("m"))
                    value *= 60000;
                else
                {
                    if (respawn.Contains("h"))
                        value *= 3600000;
                    else
                    {
                        if (respawn.Contains("d"))
                            value *= 86400000;
                    }
                }
            }

          //  for (int a = 0; a < count; a++)
           //     Spawns.Add(new L2Spawn(id, value, this, pos));
        }

        public void AddPoint(string[] loc)
        {
            int x = Convert.ToInt32(loc[0]);
            int y = Convert.ToInt32(loc[1]);
            int zmin = Convert.ToInt32(loc[2]);
            int zmax;

            try
            {
                zmax = Convert.ToInt32(loc[3]);
            }
            catch (Exception e)
            {
                Log.Error($"err in {loc[3]}. Message: {e.Message}");
                throw;
            }

            TerritoryLoc.Add(new[] { x, y, zmin, zmax });
        }

        public void InitZone()
        {
            int z1 = 0,
                z2 = 0;
            int[] x = new int[TerritoryLoc.Count];
            int[] y = new int[TerritoryLoc.Count];
            byte i = 0;
            foreach (int[] l in TerritoryLoc)
            {
                x[i] = l[0];
                y[i] = l[1];
                z1 = l[2];
                z2 = l[3];
                i++;
            }

            Territory = new ZoneNPoly(x, y, z1, z2);
        }

        public void Spawn()
        {
            //Spawns.ForEach(sp => sp.Init());
        }

        public int[] GetSpawnLocation()
        {
            for (short a = 0; a < short.MaxValue; a++) //TODO FIX
            {
                int rndx = RandomThreadSafe.Instance.Next(Territory.minX, Territory.maxX);
                int rndy = RandomThreadSafe.Instance.Next(Territory.minY, Territory.maxY);

                if (Territory.isInsideZone(rndx, rndy))
                    return new[] { rndx, rndy, Territory.getHighZ() };
            }

            Log.Error("getSpawnLocation failed after 400 tries. omg!");
            return null;
        }

        public void SunRise(bool y)
        {
           // Spawns.ForEach(sp => sp.SunRise(y));
        }
    }
}