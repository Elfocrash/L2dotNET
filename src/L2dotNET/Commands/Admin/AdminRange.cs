using System;
using System.Threading;
using System.Timers;
using log4net;
using L2dotNET.Attributes;
using L2dotNET.model.player;
using L2dotNET.model.structures.conq;
using L2dotNET.model.zones.forms;
using L2dotNET.tables.multisell;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "range")]
    class AdminRange : AAdminCommand
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AdminRange));

        private ZoneNPoly _np;

        protected internal override void Use(L2Player admin, string alias)
        {
            //  double dis = Calcs.calculateDistance(admin, admin._currentTarget, true);
            //  admin.sendMessage($"dis {dis}");

            //  foreach (L2SkillCoolTime ct in admin._reuse.Values)
            //      ct.forcedStop();

            //  admin._reuse.Clear();
            //  admin.updateReuse();

            string s = alias.Split(' ')[1];

            switch (s)
            {
                case "1":
                    admin.AbnormalBitMaskEvent = int.Parse(alias.Split(' ')[2]);
                    admin.UpdateAbnormalEventEffect();
                    break;
                case "2":
                    int listid = int.Parse(alias.Split(' ')[2]);
                    MultiSell.Instance.ShowList(admin, null, listid);
                    break;
                case "4":
                    FortressOfTheDead d = new FortressOfTheDead();
                    d.Start();
                    break;
                case "5":
                    if (_np == null)
                    {
                        int[] x = { -81166, -80913, -81952, -82554 };
                        int[] y = { 245118, 246031, 246551, 245619 };
                        _np = new ZoneNPoly(x, y, -3727, -3727);
                    }

                    int count = int.Parse(alias.Split(' ')[2]);

                    for (int i = 0; i < count; i++)
                    {
                        int[] rloc = RndXyz();
                        // NpcTable.getInstance().spawnNpc("lector", rloc[0], rloc[1], rloc[3], new Random().Next(65000));
                    }

                    break;
            }

            // admin._privateStoreType = byte.Parse(alias.Split(' ')[1]);
            // admin.broadcastUserInfo();

            // int val = int.Parse(alias.Split(' ')[1]);

            //  StatusUpdate su = new StatusUpdate(admin);
            //  su.add(val, 5000);
            //  admin.sendPacket(su);
        }

        //private System.Timers.Timer t;
        //private L2Player adm;

        private void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            //List<int> l = new List<int>();
            //l.Add(adm.ObjID);
            //adm.broadcastPacket(new MagicSkillLaunched(adm, l, 261, 1));
            //t.Enabled = false;
            //adm.sendMessage("END!");
        }

        public int[] RndXyz()
        {
            int i;
            int[] p = new int[4];
            Random rn = new Random();
            for (i = 0; i < 200; i++)
            {
                p[0] = rn.Next(_np.minX, _np.maxX);
                p[1] = rn.Next(_np.minY, _np.maxY);
                Log.Info($"rnd xy {p[0]} {p[1]}");
                if (!_np.isInsideZone(p[0], p[1]))
                    continue;

                double curdistance = -1;
                p[2] = _np.getLowZ() + 10;
                p[3] = _np.getHighZ();

                for (i = 0; i < _np._x.Length; i++)
                {
                    int p1X = _np._x[i];
                    int p1Y = _np._y[i];
                    int dx = p1X - p[0],
                        dy = p1Y - p[1];
                    double distance = Math.Sqrt((dx * dx) + (dy * dy));
                    if ((curdistance != -1) && !(distance < curdistance))
                        continue;

                    curdistance = distance;
                    p[2] = _np._z1 + 10;
                }

                return p;
            }

            return p;
        }

        public void Read()
        {
            Thread.Sleep(1000);
            // _admin.validateVisibleObjects(_admin);
        }
    }
}