using System;
using log4net;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Tables
{
    public class L2Spawn
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(L2Spawn));

        public int NpcId;
        public long Respawn;
        public System.Timers.Timer RespawnTimer;

        public L2Object Obj;
        private readonly L2Territory _zone;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly int _h;

        private const byte Mode = ModeAnytime;
        private byte _status = StatusOffline;
        public const byte ModeDayOnly = 1,
                          ModeNightOnly = 2,
                          ModeAnytime = 0,
                          StatusActive = 1,
                          StatusOffline = 2,
                          StatusInactive = 0;

        public L2Spawn(int npcId, long respawn, L2Territory zone, string pos)
        {
            this.NpcId = npcId;
            this.Respawn = respawn;
            this._zone = zone;
            if (pos != null)
            {
                _x = Convert.ToInt32(pos[0]);
                _y = Convert.ToInt32(pos[1]);
                _z = Convert.ToInt32(pos[2]);
                _h = Convert.ToInt32(pos[3]);
            }
        }

        public L2Spawn(int npcId, long respawn, string[] loc)
        {
            this.NpcId = npcId;
            this.Respawn = respawn;

            _x = Convert.ToInt32(loc[0]);
            _y = Convert.ToInt32(loc[1]);
            _z = Convert.ToInt32(loc[2]);
            _h = Convert.ToInt32(loc[3]);
        }

        public void Init()
        {
            int[] sp = null;
            if ((_x > 0) || (_y > 0))
                sp = new[] { _x, _y, _z };
            else
                try
                {
                    sp = _zone.GetSpawnLocation();
                }
                catch (Exception e)
                {
                    sp = new[] { 0, 0, 0, 0 };
                    //  throw e;
                    _log.Error($"L2Spawn: {e.Message}");
                }

            //obj = NpcTable.Instance.SpawnNpc(NpcId, sp[0], sp[1], sp[2], (zone == null) ? h : zone.rnd.Next(64000));

            if (Obj == null)
                return;

            if (Obj is L2Warrior)
                ((L2Warrior)Obj).TerritorySpawn = this;

            _status = StatusActive;
        }

        public void OnDie(L2Warrior warrior, L2Character killer)
        {
            Obj = null;
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
            Init();
        }

        public void SunRise(bool rise)
        {
            switch (Mode)
            {
                case ModeDayOnly:
                    if (!rise)
                    {
                        if (_status == StatusActive)
                            Clear();
                    }
                    else
                    {
                        if (_status != StatusActive)
                            Init();
                    }
                    break;
                case ModeNightOnly:
                    if (rise)
                    {
                        if (_status == StatusActive)
                            Clear();
                    }
                    else
                    {
                        if (_status != StatusActive)
                            Init();
                    }
                    break;
            }
        }

        private void Clear()
        {
            if (Obj != null)
                if (Obj is L2Character)
                    ((L2Character)Obj).DeleteByForce();
        }
    }
}