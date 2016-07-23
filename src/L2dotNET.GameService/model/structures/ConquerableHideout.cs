using System.Collections.Generic;
using System.Linq;
using System.Timers;
using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Communities;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Structures
{
    class ConquerableHideout
    {
        public string Name;
        public int Id;
        public int BossId,
                   Minion1Id,
                   Minion2Id,
                   MessengerId;
        public int[] Spawn1,
                     Spawn2,
                     Spawn3,
                     Spawn4;
        public bool IsActive;
        public Timer TimeWait = null,
                     TimeReg = null,
                     TimeSiege;

        public int ReputationNothing = 600;
        public int ReputationCapture = 600;
        public int ReputationLosing = 600;

        public SortedList<int, double> ClanDamage;
        public List<int[]> MobSpawns;
        private L2Character _npc1,
                            _npc2,
                            _npc3,
                            _npc4;

        public virtual void Init()
        {
            Message($"Siege registration of {Name} has begun!");
            Message("Now its open for 2 hours!");

            _npc4 = (L2Character)SpawnTable.Instance.SpawnOne(MessengerId, Spawn4[0], Spawn4[1], Spawn4[2], Spawn4[3]);
        }

        public void Message(string text)
        {
            AnnouncementManager.Instance.Announce(text);
        }

        private List<L2Character> _mobActive;

        public virtual void Start()
        {
            IsActive = true;
            Message($"Siege of {Name} has begun!");
            _npc1 = (L2Character)SpawnTable.Instance.SpawnOne(BossId, Spawn1[0], Spawn1[1], Spawn1[2], Spawn1[3]);
            _npc2 = (L2Character)SpawnTable.Instance.SpawnOne(Minion1Id, Spawn2[0], Spawn2[1], Spawn2[2], Spawn2[3]);
            _npc3 = (L2Character)SpawnTable.Instance.SpawnOne(Minion2Id, Spawn3[0], Spawn3[1], Spawn3[2], Spawn3[3]);
            _npc4 = (L2Character)SpawnTable.Instance.SpawnOne(MessengerId, Spawn4[0], Spawn4[1], Spawn4[2], Spawn4[3]);
            ClanDamage = new SortedList<int, double>();
            _mobActive = new List<L2Character>();

            foreach (L2Character o in MobSpawns.Select(sp => (L2Character)SpawnTable.Instance.SpawnOne(sp[0], sp[1], sp[2], sp[3], sp[4])))
                _mobActive.Add(o);

            TimeSiege = new Timer
                        {
                            Interval = 3600000
                        };
            // 60 минут
            TimeSiege.Elapsed += TimeSiegeEnd;

            Message($"Guards {_mobActive.Count}! 60 min left");
        }

        private void TimeSiegeEnd(object sender, ElapsedEventArgs e)
        {
            TimeSiege.Enabled = false;
            SiegeEnd(true);
        }

        public void SiegeEnd(bool trigger)
        {
            IsActive = false;
            Message($"Siege of {Name} is over.");
            if (trigger)
                Message($"Nobody won! {Name} belong to NPC until next siege.");
            else
            {
                double dmg = 0;
                int tmClanId = 0;
                foreach (int clanId in ClanDamage.Keys.Where(clanId => ClanDamage[clanId] > dmg))
                {
                    dmg = ClanDamage[clanId];
                    tmClanId = clanId;
                }

                if (tmClanId > 0)
                {
                    L2Clan cl = ClanTable.Instance.GetClan(tmClanId);
                    Message($"Now its belong to: '{cl.Name}' until next siege.");
                    bool captured = false; //todo

                    if (captured)
                    {
                        cl.UpdatePledgeNameValue(ReputationCapture);
                        cl.BroadcastToMembers(new SystemMessage(SystemMessage.SystemMessageId.ClanAddedS1SPointsToReputationScore).AddNumber(ReputationCapture));
                    }
                    else
                    {
                        cl.UpdatePledgeNameValue(ReputationNothing);
                        cl.BroadcastToMembers(new SystemMessage(SystemMessage.SystemMessageId.ClanAcquiredContestedClanHallAndS1ReputationPoints).AddNumber(ReputationNothing));
                    }
                }
                else
                {
                    Message($"Nobody won! {Name} belong to NPC until next siege.");
                    //trigger = true;
                }
            }

            _mobActive.ForEach(o => o.DeleteByForce());
        }

        public void AddDamage(int clanId, double dmg)
        {
            if (ClanDamage.ContainsKey(clanId))
                ClanDamage[clanId] += dmg;
            else
                ClanDamage.Add(clanId, dmg);
        }

        public void AddSpawn(int spawnId, int x, int y, int z, int h)
        {
            if (MobSpawns == null)
                MobSpawns = new List<int[]>();

            MobSpawns.Add(new[] { spawnId, x, y, z, h });
        }
    }
}