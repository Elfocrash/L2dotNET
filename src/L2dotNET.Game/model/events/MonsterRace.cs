using System;
using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.model.zones;
using L2dotNET.GameService.model.zones.classes;
using L2dotNET.GameService.model.zones.forms;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;
using L2dotNET.GameService.world;
using log4net;

namespace L2dotNET.GameService.model.events
{
    public class MonsterRace
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MonsterRace));
        private static volatile MonsterRace instance;
        private static object syncRoot = new object();

        public static MonsterRace Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new MonsterRace();
                        }
                    }
                }

                return instance;
            }
        }

        public string beginMusic = "S_Race", beginSound = "ItemSound2.race_start", endSound = "ItemSound2.race_end";
        public int[][] tracks;
        public MonsterRunner[] runners;
        public monster_race Zone;
        public List<int[]> zoneLoc;
        public int TICKET = 4443, TICKET_DOUBLE = 4444;

        public MonsterRace()
        {

        }

        public void Initialize()
        {
            tracks = new int[8][];
            tracks[0] = new int[] { 14107, 182287, -3586, 12107, 182287, -3586 };
            tracks[1] = new int[] { 14107, 182229, -3586, 12107, 182229, -3586 };
            tracks[2] = new int[] { 14107, 182171, -3586, 12107, 182171, -3586 };
            tracks[3] = new int[] { 14107, 182113, -3586, 12107, 182113, -3586 };
            tracks[4] = new int[] { 14107, 182055, -3586, 12107, 182055, -3586 };
            tracks[5] = new int[] { 14107, 181997, -3586, 12107, 181997, -3586 };
            tracks[6] = new int[] { 14107, 181939, -3586, 12107, 181939, -3586 };
            tracks[7] = new int[] { 14107, 181881, -3586, 12107, 181881, -3586 };

            zoneLoc = new List<int[]>();
            zoneLoc.Add(new int[] { 11296, 181024, -3800, -3300 });
            zoneLoc.Add(new int[] { 14640, 180868, -3800, -3300 });
            zoneLoc.Add(new int[] { 14692, 183332, -3800, -3300 });
            zoneLoc.Add(new int[] { 11304, 183152, -3800, -3300 });

            runners = new MonsterRunner[24];
            runners[0] = new MonsterRunner(31003, 712, 130, 70);
            runners[1] = new MonsterRunner(31004, 713, 125, 75);
            runners[2] = new MonsterRunner(31005, 714, 125, 70);
            runners[3] = new MonsterRunner(31006, 715, 120, 75);
            runners[4] = new MonsterRunner(31007, 716, 120, 65);
            runners[5] = new MonsterRunner(31008, 717, 115, 70);
            runners[6] = new MonsterRunner(31009, 718, 110, 70);
            runners[7] = new MonsterRunner(31010, 719, 115, 65);
            runners[8] = new MonsterRunner(31011, 720, 120, 60);
            runners[9] = new MonsterRunner(31012, 721, 125, 55);
            runners[10] = new MonsterRunner(31013, 722, 130, 50);
            runners[11] = new MonsterRunner(31014, 723, 135, 45);
            runners[12] = new MonsterRunner(31015, 724, 105, 65);
            runners[13] = new MonsterRunner(31016, 725, 110, 60);
            runners[14] = new MonsterRunner(31017, 726, 115, 55);
            runners[15] = new MonsterRunner(31018, 727, 120, 50);
            runners[16] = new MonsterRunner(31019, 728, 125, 45);
            runners[17] = new MonsterRunner(31020, 729, 130, 40);
            runners[18] = new MonsterRunner(31021, 730, 100, 60);
            runners[19] = new MonsterRunner(31022, 731, 95, 65);
            runners[20] = new MonsterRunner(31023, 732, 105, 55);
            runners[21] = new MonsterRunner(31024, 733, 110, 50);
            runners[22] = new MonsterRunner(31025, 734, 90, 70);
            runners[23] = new MonsterRunner(31026, 735, 100, 60);
            GenZone();
            GenNpc();

            L2World.Instance.RealiseEntry(raceManager1, null, true);
            raceManager1.onSpawn();
            L2World.Instance.RealiseEntry(raceManager2, null, true);
            raceManager2.onSpawn();

            log.Info("MonsterRace loaded.");
        }

        private L2RaceManager raceManager1, raceManager2;
        private void GenNpc()
        {
            NpcTemplate nt = NpcTable.Instance.GetNpcTemplate(30995);

            raceManager1 = new L2RaceManager();
            raceManager1.setTemplate(nt);
            raceManager1.X = 13691;
            raceManager1.Y = 182627;
            raceManager1.Z = -3561;
            raceManager1.Heading = 49152;

            raceManager2 = new L2RaceManager();
            raceManager2.setTemplate(nt);
            raceManager2.X = 12962;
            raceManager2.Y = 181603;
            raceManager2.Z = -3561;
            raceManager2.Heading = 16384;
        }

        public int currentRaceId = 1, timeToStart, maxSpd = 0, rnd, raceTimeProgress;
        public System.Timers.Timer raceStartTime, raceInProgress;
        Random rn = new Random();
        public void startRace()
        {
            maxSpd = 0;
            timeToStart = 60;
            rnd = rn.Next(17);
            for (byte a = 0; a < 8; a++)
            {
                int spd = runners[a + rnd].Prepare(tracks[a], a);
                if (spd > maxSpd)
                    maxSpd = spd;
            }

            int[] timeRange = new int[8];
            for (byte a = 0; a < 8; a++)
                timeRange[a] = runners[a + rnd].finishTime;

            raceTimeProgress = timeRange.Max() * 2;

            foreach (L2Object obj in Zone.ObjectsInside.Values)
            {
                if (obj is L2Player)
                    for (byte a = 0; a < 8; a++)
                        obj.sendPacket(new NpcInfoMonrace(runners[a + rnd]));
            }

            if (raceStartTime == null)
            {
                raceStartTime = new System.Timers.Timer();
                raceStartTime.Interval = 1000;
                raceStartTime.Elapsed += new System.Timers.ElapsedEventHandler(RaceTimeCounter);
            }

            raceStartTime.Enabled = true;
            status = 1;
        }

        public byte status = 0;

        private void RaceTimeCounter(object sender, System.Timers.ElapsedEventArgs e)
        {
            SystemMessage sm = null;
            switch (timeToStart)
            {
                case 300:
                case 240:
                case 180:
                case 120:
                case 60:
                    sm = new SystemMessage(820);//Monster Race $s2 will begin in $s1 minute(s)!
                    sm.AddNumber(timeToStart / 60);
                    sm.AddNumber(currentRaceId);
                    break;
                case 30:
                    sm = new SystemMessage(821);//Monster Race $s1 will begin in 30 seconds!
                    sm.AddNumber(currentRaceId);
                    break;
                case 6:
                    sm = new SystemMessage(822);//Monster Race $s1 is about to begin! Countdown in five seconds!
                    sm.AddNumber(currentRaceId);
                    break;
                case 5:
                case 4:
                case 3:
                case 2:
                case 1:
                    sm = new SystemMessage(823);//The race will begin in $s1 second(s)!
                    sm.AddNumber(timeToStart);
                    break;
                case 0:
                    sm = new SystemMessage(824);//They're off!
                    raceStartTime.Enabled = false;

                    foreach (L2Object obj in Zone.ObjectsInside.Values)
                    {
                        if (obj is L2Player)
                        {
                            for (byte a = 0; a < 8; a++)
                                obj.sendPacket(new CharMoveToLocationMonrace(runners[a + rnd]));

                            obj.sendPacket(new PlaySound(beginSound));
                            obj.sendPacket(new PlaySound(beginMusic, true));
                        }
                    }

                    if (raceInProgress == null)
                    {
                        raceInProgress = new System.Timers.Timer();
                        raceInProgress.Interval = raceTimeProgress;
                        raceInProgress.Elapsed += new System.Timers.ElapsedEventHandler(RaceEnd);
                    }

                    raceInProgress.Enabled = true;
                    status = 2;
                    break;
            }

            if (sm != null)
                Zone.broadcastPacket(sm);

            timeToStart--;
        }

        private void RaceEnd(object sender, System.Timers.ElapsedEventArgs e)
        {
            status = 3;
            SystemMessage sm = new SystemMessage(825); //Monster Race $s1 is finished!
            sm.AddNumber(currentRaceId);
            Zone.broadcastPacket(sm);

            byte firstLine = 0, secondLine = 0;
            int max = int.MaxValue;
            for (byte a = 0; a < 8; a++)
            {
                if (max > runners[a + rnd].finishTime)
                {
                    max = runners[a + rnd].finishTime;
                    firstLine = runners[a + rnd].lineId;
                }
            }

            max = int.MaxValue;
            for (byte a = 0; a < 8; a++)
            {
                if (runners[a + rnd].lineId == firstLine)
                    continue;

                if (max > runners[a + rnd].finishTime)
                {
                    max = runners[a + rnd].finishTime;
                    secondLine = runners[a + rnd].lineId;
                }
            }

            //First prize goes to the player in lane $s1. Second prize goes to the player in lane $s2.
            sm = new SystemMessage(826);
            sm.AddNumber(firstLine);
            sm.AddNumber(secondLine);

            runners[firstLine - 1 + rnd].winCount++;
            runners[secondLine - 1 + rnd].winCount++;

            foreach (L2Object obj in Zone.ObjectsInside.Values)
                if (obj is L2Player)
                {
                    obj.sendPacket(sm);
                    obj.sendPacket(new PlaySound(endSound));
                    for (byte a = 0; a < 8; a++)
                        obj.sendPacket(new DeleteObject(runners[a + rnd].id));
                }

            currentRaceId++;
            raceInProgress.Enabled = false;
        }

        public void GenZone()
        {
            Zone = new monster_race();
            ZoneTemplate template = new ZoneTemplate();
            template.Name = "monsterRace";
            template.Type = ZoneTemplate.ZoneType.monster_race;
            template.setRange(zoneLoc);

            Zone.Name = template.Name;
            Zone.Template = template;
            Zone.Territory = new ZoneNPoly(template._x, template._y, template._z1, template._z2);

            for (int i = 0; i < template._x.Length; i++)
            {
                L2WorldRegion region = L2World.Instance.GetRegion(template._x[i], template._y[i]);
                if (region != null)
                {
                    //region._zoneManager.addZone(Zone);
                }
                else
                {
                    log.Info($"AreaTable[hideout]: null region at { template._x[i] } { template._y[i] } for zone { Zone.Name }");
                }
            }
        }

        public void OnBypass(L2Player player, npcs.L2Citizen npc, string cmd)
        {
            if (cmd.Equals("_mrvhome"))
            {
                player.ShowHtm("mr_keeper.htm", npc);
            }
            else if (cmd.Equals("_mrvw01"))//View Odds
            {

            }
            else if (cmd.Equals("_mrvw20"))//View Monster Information
            {
                if (status == 1)
                {
                    NpcHtmlMessage htm = new NpcHtmlMessage(player, "mr_vw20.htm", npc.ObjID);

                    for (byte a = 0; a < 8; a++)
                    {
                        MonsterRunner mr = runners[a + rnd];

                        htm.replace("<?NAME" + (a + 1) + "?>", "&$" + mr.sys_string + ";");
                        htm.replace("<?COND" + (a + 1) + "?>", 0);
                        htm.replace("<?WIN" + (a + 1) + "?>", (int)(mr.winCount / mr.runCount * 100));
                    }

                    htm.replace("<?LINK_HOME?>", "bypass _mrvhome");
                    player.sendPacket(htm);
                }
            }
            else if (cmd.Equals("_mrbu01"))//Purchase Ticket
            {
                if (status == 1)
                {
                    NpcHtmlMessage htm = new NpcHtmlMessage(player, "mr_buy1.htm", npc.ObjID);
                    htm.replace("<?RACE_ID?>", currentRaceId);
                    for (byte a = 0; a < 8; a++)
                    {
                        MonsterRunner mr = runners[a + rnd];

                        htm.replace("<?SEL" + (a + 1) + "?>", "bypass _mrbu2 " + a + " 1");
                        htm.replace("<?NAME" + (a + 1) + "?>", "&$" + mr.sys_string + ";");
                        htm.replace("<?COND" + (a + 1) + "?>", 0);
                        htm.replace("<?WIN" + (a + 1) + "?>", (int)(mr.winCount / mr.runCount * 100));
                    }

                    htm.replace("<?LINK_HOME?>", "bypass _mrvhome");
                    htm.replace("<?SEL_ID?>", "");
                    player.sendPacket(htm);
                }
            }
            else if (cmd.Equals("_mrsl01"))//Calculate Winnings
            {

            }
            else if (cmd.Equals("_mrvw31"))//View Past Results
            {

            }
            else if (cmd.StartsWith("_mrbu2"))
            {
                byte line = Convert.ToByte(cmd.Split(' ')[1]);
                byte cost = Convert.ToByte(cmd.Split(' ')[2]);

                int adena = 0;
                switch (cost)
                {
                    case 1: adena = 100; break;
                    case 2: adena = 500; break;
                    case 3: adena = 1000; break;
                    case 4: adena = 5000; break;
                    case 5: adena = 10000; break;
                    case 6: adena = 20000; break;
                    case 7: adena = 50000; break;
                    case 8: adena = 100000; break;
                }

                MonsterRunner mr = runners[line + rnd];
                NpcHtmlMessage htm = new NpcHtmlMessage(player, "mr_buy2.htm", npc.ObjID);
                htm.replace("<?RACE_ID?>", currentRaceId);
                htm.replace("<?LANE?>", (line + 1));
                htm.replace("<?NAME1?>", "&$" + mr.sys_string + ";");
                htm.replace("<?COND1?>", 0);
                htm.replace("<?WIN1?>", (int)(mr.winCount / mr.runCount * 100));

                for (byte a = 1; a <= 8; a++)
                    htm.replace("<?LINK_BUY_" + a + "?>", "bypass _mrbu2 " + line + " " + a);

                htm.replace("<?SEL_ID?>", adena);
                htm.replace("<?LINK_NEXT?>", "bypass _mrbu3 " + line + " " + cost);
                player.sendPacket(htm);
            }
            else if (cmd.StartsWith("_mrbu3"))
            {
                byte line = Convert.ToByte(cmd.Split(' ')[1]);
                byte cost = Convert.ToByte(cmd.Split(' ')[2]);

                int adena = 0;
                switch (cost)
                {
                    case 1: adena = 100; break;
                    case 2: adena = 500; break;
                    case 3: adena = 1000; break;
                    case 4: adena = 5000; break;
                    case 5: adena = 10000; break;
                    case 6: adena = 20000; break;
                    case 7: adena = 50000; break;
                    case 8: adena = 100000; break;
                }

                MonsterRunner mr = runners[line + rnd];
                NpcHtmlMessage htm = new NpcHtmlMessage(player, "mr_buy3.htm", npc.ObjID);
                htm.replace("<?RACE_ID?>", currentRaceId);
                htm.replace("<?LANE?>", (line + 1));
                htm.replace("<?NAME1?>", "&$" + mr.sys_string + ";");
                htm.replace("<?COND1?>", 0);
                htm.replace("<?WIN1?>", (int)(mr.winCount / mr.runCount * 100));

                htm.replace("<?BUY_AMOUNT?>", adena);
                htm.replace("<?TAX_AMOUNT?>", 0);
                htm.replace("<?SUM_AMOUNT?>", adena);

                htm.replace("<?LINK_HOME?>", "bypass _mrvhome");
                htm.replace("<?LINK_NEXT?>", "bypass _mrbu4 " + line + " " + cost);
                player.sendPacket(htm);
            }
            else if (cmd.StartsWith("_mrbu4"))
            {
                byte line = Convert.ToByte(cmd.Split(' ')[1]);
                byte cost = Convert.ToByte(cmd.Split(' ')[2]);

                int adena = 0;
                switch (cost)
                {
                    case 1: adena = 100; break;
                    case 2: adena = 500; break;
                    case 3: adena = 1000; break;
                    case 4: adena = 5000; break;
                    case 5: adena = 10000; break;
                    case 6: adena = 20000; break;
                    case 7: adena = 50000; break;
                    case 8: adena = 100000; break;
                }

                if (player.getAdena() < adena)
                {
                    player.sendSystemMessage(279);//You do not have enough adena.
                    return;
                }

                player.reduceAdena(adena, true, true);

                L2Item ticket = new L2Item(ItemTable.Instance.GetItem(TICKET));
                ticket.Location = L2Item.L2ItemLocation.inventory;
                ticket.CustomType1 = line + 1;
                ticket.CustomType2 = adena / 100;
                ticket.Enchant = currentRaceId;

                player.Inventory.addItem(ticket, true, true);
            }
        }
    }

    public class MonsterRunner
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MonsterRace));

        public int id, npcId, sys_string, max_speed, min_speed;
        public MonsterRunner(int npcId, int sys_string, int max_speed, int min_speed)
        {
            this.npcId = npcId;
            this.npcTemplate = NpcTable.Instance.GetNpcTemplate(npcId);
            this.sys_string = sys_string;
            this.max_speed = max_speed;
            this.min_speed = min_speed;
            this.id = IdFactory.Instance.nextId();
        }

        public int x, y, z, dx, dy, dz;
        public int heading = 32500, cur_speed, finishTime;
        public byte lineId;
        public npcs.NpcTemplate npcTemplate;

        public long runCount = 0, winCount = 0;

        public int Prepare(int[] track, byte line)
        {
            x = track[0];
            y = track[1];
            z = track[2];
            dx = track[3];
            dy = track[4];
            dz = track[5];
            lineId = line;
            lineId += 1;

            cur_speed = new Random().Next(min_speed, max_speed);

            finishTime = (1 + (int)(10 * 2000 / cur_speed)) * 100;
            runCount++;
            log.Info($"runner #{ id } spd { cur_speed } , ms { finishTime }");
            return cur_speed;
        }
    }
}
