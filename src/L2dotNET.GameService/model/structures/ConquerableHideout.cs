using System.Collections.Generic;
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
        public string name;
        public int id;
        public int BossId,
                   Minion1Id,
                   Minion2Id,
                   MessengerId;
        public int[] spawn1,
                     spawn2,
                     spawn3,
                     spawn4;
        public bool isActive;
        public Timer TimeWait = null,
                     TimeReg = null,
                     TimeSiege;

        public int ReputationNothing = 600;
        public int ReputationCapture = 600;
        public int ReputationLosing = 600;

        public SortedList<int, double> clanDamage;
        public List<int[]> mobSpawns;
        private L2Character npc1,
                            npc2,
                            npc3,
                            npc4;

        public virtual void init()
        {
            Message("Siege registration of " + name + " has begun!");
            Message("Now its open for 2 hours!");

            npc4 = (L2Character)SpawnTable.Instance.SpawnOne(MessengerId, spawn4[0], spawn4[1], spawn4[2], spawn4[3]);
        }

        public void Message(string text)
        {
            AnnouncementManager.Instance.Announce(text);
        }

        private List<L2Character> mobActive;

        public virtual void start()
        {
            isActive = true;
            Message("Siege of " + name + " has begun!");
            npc1 = (L2Character)SpawnTable.Instance.SpawnOne(BossId, spawn1[0], spawn1[1], spawn1[2], spawn1[3]);
            npc2 = (L2Character)SpawnTable.Instance.SpawnOne(Minion1Id, spawn2[0], spawn2[1], spawn2[2], spawn2[3]);
            npc3 = (L2Character)SpawnTable.Instance.SpawnOne(Minion2Id, spawn3[0], spawn3[1], spawn3[2], spawn3[3]);
            npc4 = (L2Character)SpawnTable.Instance.SpawnOne(MessengerId, spawn4[0], spawn4[1], spawn4[2], spawn4[3]);
            clanDamage = new SortedList<int, double>();
            mobActive = new List<L2Character>();

            foreach (int[] sp in mobSpawns)
            {
                L2Character o = (L2Character)SpawnTable.Instance.SpawnOne(sp[0], sp[1], sp[2], sp[3], sp[4]);
                mobActive.Add(o);
            }

            TimeSiege = new Timer();
            TimeSiege.Interval = 3600000; // 60 минут
            TimeSiege.Elapsed += new ElapsedEventHandler(TimeSiegeEnd);

            Message("Guards " + mobActive.Count + "! 60 min left");
        }

        private void TimeSiegeEnd(object sender, ElapsedEventArgs e)
        {
            TimeSiege.Enabled = false;
            SiegeEnd(true);
        }

        public void SiegeEnd(bool trigger)
        {
            isActive = false;
            Message("Siege of " + name + " is over.");
            if (trigger)
                Message("Nobody won! " + name + " belong to NPC until next siege.");
            else
            {
                double dmg = 0;
                int tmClanId = 0;
                foreach (int clanId in clanDamage.Keys)
                    if (clanDamage[clanId] > dmg)
                    {
                        dmg = clanDamage[clanId];
                        tmClanId = clanId;
                    }

                if (tmClanId > 0)
                {
                    L2Clan cl = ClanTable.Instance.GetClan(tmClanId);
                    Message("Now its belong to: '" + cl.Name + "' until next siege.");
                    bool captured = false; //todo

                    if (captured)
                    {
                        cl.UpdatePledgeNameValue(ReputationCapture);
                        cl.broadcastToMembers(new SystemMessage(SystemMessage.SystemMessageId.CLAN_ADDED_S1S_POINTS_TO_REPUTATION_SCORE).AddNumber(ReputationCapture));
                    }
                    else
                    {
                        cl.UpdatePledgeNameValue(ReputationNothing);
                        cl.broadcastToMembers(new SystemMessage(SystemMessage.SystemMessageId.CLAN_ACQUIRED_CONTESTED_CLAN_HALL_AND_S1_REPUTATION_POINTS).AddNumber(ReputationNothing));
                    }
                }
                else
                {
                    Message("Nobody won! " + name + " belong to NPC until next siege.");
                    //trigger = true;
                }
            }

            foreach (L2Character o in mobActive)
                o.DeleteByForce();
        }

        public void addDamage(int clanId, double dmg)
        {
            if (clanDamage.ContainsKey(clanId))
                clanDamage[clanId] += dmg;
            else
                clanDamage.Add(clanId, dmg);
        }

        public void addSpawn(int spawnId, int x, int y, int z, int h)
        {
            if (mobSpawns == null)
                mobSpawns = new List<int[]>();

            mobSpawns.Add(new[] { spawnId, x, y, z, h });
        }
    }
}