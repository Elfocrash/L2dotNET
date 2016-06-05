using System;
using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.Model.Zones.Forms;

namespace L2dotNET.GameService.Tables
{
    public class L2Territory
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(L2Territory));
        public string name;
        public string controller;
        public bool start_active;
        public List<L2Spawn> spawns = new List<L2Spawn>();
        public List<int[]> territoryLoc = new List<int[]>();
        public Random rnd = new Random();
        public ZoneNPoly territory;

        public void AddNpc(int id, int count, string respawn, string pos)
        {
            long value = Convert.ToInt32(respawn.Remove(respawn.Length - 1));

            if (respawn.Contains("s"))
                value *= 1000;
            else if (respawn.Contains("m"))
                value *= 60000;
            else if (respawn.Contains("h"))
                value *= 3600000;
            else if (respawn.Contains("d"))
                value *= 86400000;

            for (int a = 0; a < count; a++)
                spawns.Add(new L2Spawn(id, value, this, pos));
        }

        public void AddPoint(string[] loc)
        {
            int x = Convert.ToInt32(loc[0]);
            int y = Convert.ToInt32(loc[1]);
            int zmin = Convert.ToInt32(loc[2]);
            int zmax = zmin;

            try
            {
                zmax = Convert.ToInt32(loc[3]);
            }
            catch (Exception asd)
            {
                log.Error($"err in {loc[3]}");
                throw asd;
            }

            territoryLoc.Add(new int[] { x, y, zmin, zmax });
        }

        public void InitZone()
        {
            int z1 = 0,
                z2 = 0;
            int[] x = new int[territoryLoc.Count];
            int[] y = new int[territoryLoc.Count];
            byte i = 0;
            foreach (int[] l in territoryLoc)
            {
                x[i] = l[0];
                y[i] = l[1];
                z1 = l[2];
                z2 = l[3];
                i++;
            }

            territory = new ZoneNPoly(x, y, z1, z2);
        }

        public void Spawn()
        {
            foreach (L2Spawn sp in spawns)
                sp.init();
        }

        public int[] getSpawnLocation()
        {
            for (short a = 0; a < short.MaxValue; a++) //TODO FIX
            {
                int rndx = rnd.Next(territory.minX, territory.maxX);
                int rndy = rnd.Next(territory.minY, territory.maxY);

                if (territory.isInsideZone(rndx, rndy))
                    return new int[] { rndx, rndy, territory.getHighZ() };
            }

            log.Error("getSpawnLocation failed after 400 tries. omg!");
            return null;
        }

        public void SunRise(bool y)
        {
            foreach (L2Spawn sp in spawns)
                sp.SunRise(y);
        }
    }
}