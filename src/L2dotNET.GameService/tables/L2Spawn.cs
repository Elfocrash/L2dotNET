using System;
using log4net;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Tables
{
    public class L2Spawn
    {
        private readonly ILog log = LogManager.GetLogger(typeof(L2Spawn));

        public int NpcId;
        public long Respawn;
        public System.Timers.Timer RespawnTimer;

        public L2Object obj;
        private readonly L2Territory zone;
        private readonly int x;
        private readonly int y;
        private readonly int z;
        private readonly int h;

        private const byte Mode = MODE_ANYTIME;
        private byte STATUS = STATUS_OFFLINE;
        public const byte MODE_DAY_ONLY = 1,
                          MODE_NIGHT_ONLY = 2,
                          MODE_ANYTIME = 0,
                          STATUS_ACTIVE = 1,
                          STATUS_OFFLINE = 2,
                          STATUS_INACTIVE = 0;

        public L2Spawn(int NpcId, long Respawn, L2Territory zone, string pos)
        {
            this.NpcId = NpcId;
            this.Respawn = Respawn;
            this.zone = zone;
            if (pos != null)
            {
                x = Convert.ToInt32(pos[0]);
                y = Convert.ToInt32(pos[1]);
                z = Convert.ToInt32(pos[2]);
                h = Convert.ToInt32(pos[3]);
            }
        }

        public L2Spawn(int NpcId, long Respawn, string[] loc)
        {
            this.NpcId = NpcId;
            this.Respawn = Respawn;

            x = Convert.ToInt32(loc[0]);
            y = Convert.ToInt32(loc[1]);
            z = Convert.ToInt32(loc[2]);
            h = Convert.ToInt32(loc[3]);
        }

        public void init()
        {
            int[] sp = null;
            if ((x > 0) || (y > 0))
                sp = new[] { x, y, z };
            else
                try
                {
                    sp = zone.getSpawnLocation();
                }
                catch (Exception e)
                {
                    sp = new[] { 0, 0, 0, 0 };
                    //  throw e;
                    log.Error($"L2Spawn: {e.Message}");
                }

            //obj = NpcTable.Instance.SpawnNpc(NpcId, sp[0], sp[1], sp[2], (zone == null) ? h : zone.rnd.Next(64000));

            if (obj == null)
                return;

            if (obj is L2Warrior)
                ((L2Warrior)obj).TerritorySpawn = this;

            STATUS = STATUS_ACTIVE;
        }

        public void onDie(L2Warrior warrior, L2Character killer)
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