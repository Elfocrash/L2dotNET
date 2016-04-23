using System;
using System.Threading;
using L2dotNET.Game.managers;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.npcs;
using L2dotNET.Game.model.zones.forms;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.world;
using L2dotNET.Game.model.skills2;
using L2dotNET.Game.model.skills2.effects;
using L2dotNET.Game.model.events;
using L2dotNET.Game.model.vehicles;
using System.Collections.Generic;
using L2dotNET.Game.model.structures.conq;
using L2dotNET.Game.tables.multisell;

namespace L2dotNET.Game.tables.admin
{
    class AA_range : _adminAlias
    {

        public AA_range()
        {
            cmd = "range";
        }
        ZoneNPoly np;
        protected internal override void use(L2Player admin, string alias)
        {
          //  double dis = Calcs.calculateDistance(admin, admin._currentTarget, true);
          //  admin.sendMessage("dis "+dis);

          //  foreach (L2SkillCoolTime ct in admin._reuse.Values)
          //  {
          //      ct.forcedStop();
          //  }
          //  admin._reuse.Clear();
          //  admin.updateReuse();

            string s = alias.Split(' ')[1];

            if (s == "1")
            {
                admin.AbnormalBitMaskEvent = int.Parse(alias.Split(' ')[2]);
                admin.updateAbnormalEventEffect();
            }
            else if (s == "2")
            {
                int listid = int.Parse(alias.Split(' ')[2]);
                MultiSell.getInstance().showList(admin, null, listid);
            }
            else if (s == "4")
            {
                FortressOfTheDead d = new FortressOfTheDead();
                d.start();
            }
            else if (s == "5")
            {
                if (np == null)
                {
                    int[] x = new int[] { -81166, -80913, -81952, -82554 };
                    int[] y = new int[] { 245118, 246031, 246551, 245619 };
                    np = new ZoneNPoly(x, y, -3727, -3727);
                }

                int count = int.Parse(alias.Split(' ')[2]);

                for (int i = 0; i < count; i++)
                {
                    int[] rloc = this.rndXYZ();
                   // NpcTable.getInstance().spawnNpc("lector", rloc[0], rloc[1], rloc[3], new Random().Next(65000));
                }

            }
            else if (s == "6")
            {
                L2Citizen npc = (L2Citizen)admin.CurrentTarget;
                if (npc.Template.DropData == null)
                    admin.sendMessage("no drops at this npc");
                else
                    npc.Template.DropData.showInfo(admin);
            }
            else if (s == "7")
            {
                L2Citizen npc = (L2Citizen)admin.CurrentTarget;
                if (npc.Template.DropData == null)
                    admin.sendMessage("no drops at this npc");
                else
                    npc.Template.roll_drops(npc, admin);
            }
            else if (s == "8")
            {
                L2Citizen npc = (L2Citizen)admin.CurrentTarget;
                if (npc.Template.DropData == null)
                    admin.sendMessage("no drops at this npc");
                else
                {
                    npc.doDie(admin, false);
                    npc.Template.roll_drops(npc, admin);
                }
            }
            else if (s == "9")
            {
                admin.destx = admin.X +200;
                admin.desty = admin.Y +200;
                admin.destz = admin.Z;
                admin.sendPacket(new CharMoveToLocation(admin));
            }
            else if (s == "10")
            {
                InstanceManager.getInstance().create(1, admin);
            }
            else if (s == "11")
            {
                admin.Boat.destx = -121385;
                admin.Boat.desty = 261660;
                admin.Boat.destz = -3610;
                admin.Boat.OnRoute = true;
                admin.Boat.broadcastPacket(new VehicleStarted(admin.Boat.ObjID, 1));
                admin.Boat.broadcastPacket(new VehicleDeparture(admin.Boat, 400, 1800));
            }
            else if (s == "12")
            {
                L2Boat boat = new L2Boat();
                boat.X = -96622;
                boat.Y = 261660;
                boat.Z = -3610;

                L2World.Instance.RealiseEntry(boat, null, true);
                boat.onSpawn();
            }
            else if (s == "13")
            {
                MonsterRace.getInstance().startRace();
            }
            else if (s == "14")
            {
                L2Airship ship = new L2Airship();
                ship.X = -96622;
                ship.Y = 261660;
                ship.Z = -2610;

                L2World.Instance.RealiseEntry(ship, null, true);
                ship.onSpawn();
                admin.Airship = ship;
            }
            else if (s == "15")
            {
                admin.Airship.destx = -88999;
                admin.Airship.desty = 257167;
                admin.Airship.destz = -2610;
                admin.Airship.OnRoute = true;
                admin.Airship.broadcastPacket(new ExMoveToLocationAirShip(admin.Airship));
            }
            else if (s == "16")
            {
               // admin.Airship.CaptainId = admin.ObjID;
                admin.BoatX = admin.Airship.X - 0x16e;
                admin.BoatY = admin.Airship.Y;
                admin.BoatZ = admin.Airship.Z - 0x6b;
                admin.Airship.CaptainId = admin.ObjID;
               // admin.Airship.HelmId = admin.Inventory.getWeaponObjId();
                admin.Airship.broadcastPacket(new ExGetOnAirShip(admin));
                admin.Airship.broadcastUserInfo();
               // admin.X = admin.Airship.X;
               // admin.Y = admin.Airship.Y;
              //  admin.Z = admin.Airship.Z;
            }
            else if (s == "17")
            {
                admin.Mount(NpcTable.getInstance().getNpcTemplate(13146));
            }
            else if (s == "18")
            {
                int count = int.Parse(alias.Split(' ')[2]);
                TransformManager.getInstance().transformTo(count, admin, 30);
            }
            else if (s == "19")
            {
                admin.sstt = int.Parse(alias.Split(' ')[2]);
                admin.broadcastUserInfo();
            }
            else if (s == "20")
            {
                int sx = int.Parse(alias.Split(' ')[2]);
                admin.broadcastPacket(new MagicSkillUse(admin, admin, sx, 1, 0));
            }
            else if (s == "21")
            {
                int sx = int.Parse(alias.Split(' ')[2]);
                admin.broadcastPacket(new MagicSkillUse(admin, admin, 261, 1, 1000, sx));
                adm = admin;
                if (t == null)
                {
                    t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
                }

                t.Interval = 900;
                t.Enabled = true;
            }
            else if (s == "22")
            {
                adm = admin;
                L2Warrior war = (L2Warrior)NpcTable.getInstance().spawnNpc(21003, 14107, 182287, -3586, 32500);

                war.destx = 13107;
                war.desty = 182287;
                war.destz = -3586;
                admin.sendPacket(new CharMoveToLocation(war));
                war.dtstart = DateTime.Now;
                admin.ChangeTarget(war);

                if (t == null)
                {
                    t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
                }

                t.Interval = 2000 * 12;
                t.Enabled = true;
            }
           // admin._privateStoreType = byte.Parse(alias.Split(' ')[1]);
           // admin.broadcastUserInfo();
            

           // int val = int.Parse(alias.Split(' ')[1]);

          //  StatusUpdate su = new StatusUpdate(admin);
          //  su.add(val, 5000);
          //  admin.sendPacket(su);
        }
        System.Timers.Timer t;
        L2Player adm;

        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //List<int> l = new List<int>();
            //l.Add(adm.ObjID);
            //adm.broadcastPacket(new MagicSkillLaunched(adm, l, 261, 1));
            //t.Enabled = false;
            adm.sendMessage("END!");
        }

        public int[] rndXYZ()
        {
            int i; int[] p = new int[4];
            Random rn = new Random();
            for (i = 0; i < 200; i++)
            {
                p[0] = rn.Next(np.minX, np.maxX);
                p[1] = rn.Next(np.minY, np.maxY);
                Console.WriteLine("rnd xy " + p[0] + " " + p[1]);
                if (np.isInsideZone(p[0], p[1]))
                {
                    double curdistance = -1;
                    p[2] = np.getLowZ() + 10;
                    p[3] = np.getHighZ();

                    for (i = 0; i < np._x.Length; i++)
                    {
                        int p1x = np._x[i];
                        int p1y = np._y[i];
                        long dx = p1x - p[0], dy = p1y - p[1];
                        double distance = Math.Sqrt(dx * dx + dy * dy);
                        if (curdistance == -1 || distance < curdistance)
                        {
                            curdistance = distance;
                            p[2] = np._z1 + 10;
                        }
                    }

                    return p;
                }
            }

            return p;
        }

        public void read()
        {
            Thread.Sleep(1000);
           // _admin.validateVisibleObjects(_admin);
        }
    }
}
