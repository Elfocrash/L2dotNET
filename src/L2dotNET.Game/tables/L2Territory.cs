using System;
using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.model.zones.forms;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.tables
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
                log.Error($"err in { loc[3] }");
                throw asd;
            }

            territoryLoc.Add(new int[] { x, y, zmin, zmax });
        }

        public void InitZone()
        {
            int z1 = 0, z2 = 0;
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

    public class L2Spawn
    {
        public int NpcId;
        public long Respawn;
        public System.Timers.Timer RespawnTimer;

        public L2Object obj;
        private readonly L2Territory zone;
        private readonly int x = 0;
        private readonly int y = 0;
        private readonly int z = 0;
        private readonly int h = 0;

        private readonly byte Mode = MODE_ANYTIME;
        private byte STATUS = STATUS_OFFLINE;
        public const byte MODE_DAY_ONLY = 1, MODE_NIGHT_ONLY = 2, MODE_ANYTIME = 0, STATUS_ACTIVE = 1, STATUS_OFFLINE = 2, STATUS_INACTIVE = 0;

        public L2Spawn(int NpcId, long Respawn, L2Territory zone, string pos)
        {
            this.NpcId = NpcId;
            this.Respawn = Respawn;
            this.zone = zone;
            if (pos != null)
            {
                this.x = Convert.ToInt32(pos[0]);
                this.y = Convert.ToInt32(pos[1]);
                this.z = Convert.ToInt32(pos[2]);
                this.h = Convert.ToInt32(pos[3]);
            }
        }

        public L2Spawn(int NpcId, long Respawn, string[] loc)
        {
            this.NpcId = NpcId;
            this.Respawn = Respawn;

            this.x = Convert.ToInt32(loc[0]);
            this.y = Convert.ToInt32(loc[1]);
            this.z = Convert.ToInt32(loc[2]);
            this.h = Convert.ToInt32(loc[3]);
        }

        public void init()
        {
            int[] sp = null;
            if (x > 0 || y > 0)
                sp = new int[] { x, y, z };
            else
                try
                {
                    sp = zone.getSpawnLocation();
                }
                catch (Exception asd)
                {
                    sp = new int[] { 0, 0, 0, 0 };
                    //  throw asd;
                }

            obj = NpcTable.Instance.SpawnNpc(NpcId, sp[0], sp[1], sp[2], (zone == null) ? h : zone.rnd.Next(64000));

            if (obj == null)
                return;

            if (obj is L2Warrior)
                ((L2Warrior)obj).TerritorySpawn = this;

            STATUS = STATUS_ACTIVE;
        }

        public void onDie(L2Warrior warrior, world.L2Character killer)
        {
            obj = null;
            if (RespawnTimer == null)
            {
                RespawnTimer = new System.Timers.Timer();
                RespawnTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnRespawnTime);
            }

            RespawnTimer.Interval = Respawn;
            RespawnTimer.Enabled = true;
        }

        private void OnRespawnTime(object sender, System.Timers.ElapsedEventArgs e)
        {
            RespawnTimer.Enabled = false;
            init();
        }

        public void SunRise(bool rise)
        {
            switch (Mode)
            {
                case MODE_DAY_ONLY:
                    if (!rise)
                    {
                        if (STATUS == STATUS_ACTIVE)
                            clear();
                    }
                    else
                    {
                        if (STATUS != STATUS_ACTIVE)
                            init();
                    }
                    break;
                case MODE_NIGHT_ONLY:
                    if (rise)
                    {
                        if (STATUS == STATUS_ACTIVE)
                            clear();
                    }
                    else
                    {
                        if (STATUS != STATUS_ACTIVE)
                            init();
                    }
                    break;
            }
        }

        private void clear()
        {
            if (obj != null)
                if (obj is L2Character)
                    ((L2Character)obj).DeleteByForce();
        }
    }
}
