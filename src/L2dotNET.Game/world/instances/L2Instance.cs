using System;
using System.Collections.Generic;
using System.Timers;
using L2dotNET.Game.model.player.partials;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.world.instances
{
    public class L2Instance
    {
        public InstanceTemplate Template;
        public int ServerID;

        public Timer _timer;
        public DateTime TimeEnd;
        public int x, y, z;

        public SortedList<int, L2Player> _players = new SortedList<int, L2Player>();

        public L2Instance(InstanceTemplate tempalte)
        {
            this.Template = tempalte;
        }

        public bool createFailed(L2Player player)
        {
            if (Template.startFailed(player))
                return true;

            if (Template.minParty != -1)
            {
                int cnt = 1;
                if (player.Party != null)
                    cnt += player.Party.Members.Count;

                if (cnt < Template.minParty)
                {
                    //You must have a minimum of ($s1) people to enter this Instant Zone. Your request for entry is denied.
                    SystemMessage sm = new SystemMessage(2793);
                    sm.addNumber(Template.minParty);
                    player.sendPacket(sm);
                    return true;
                }
            }

            if (Template.ReuseActive)
            {
                //Instant zone: $s1's entry has been restricted. You can check the next possible entry time by using the command "/instancezone."
                foreach (db_InstanceReuse db in player.InstanceReuse.Values)
                {
                    if (db.id != Template.ClientId)
                        continue;

                    DateTime dt = DateTime.Now;
                    TimeSpan ts = db.dt - dt;
                    if (ts.TotalMilliseconds > 0)
                    {
                        SystemMessage sm = new SystemMessage(2720);
                        sm.addString(player.Name);
                        player.sendPacket(sm);
                        return true;
                    }
                }

                if (player.Party != null)
                {
                    foreach (L2Player pl in player.Party.Members)
                    {
                        if (pl.ObjID == player.ObjID)
                            continue;

                        foreach (db_InstanceReuse db in pl.InstanceReuse.Values)
                        {
                            if (db.id != Template.ClientId)
                                continue;

                            DateTime dt = DateTime.Now;
                            TimeSpan ts = db.dt - dt;
                            if (ts.TotalMilliseconds > 0)
                            {
                                SystemMessage sm = new SystemMessage(2720);
                                sm.addPlayerName(pl.Name);
                                player.sendPacket(sm);
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void create(L2Player player)
        {
            if (Template.ActionTime != -1)
            {
                TimeEnd = DateTime.Now.AddSeconds(Template.ActionTime);
                _timer = new Timer(Template.ActionTime * 1000);
                _timer.Interval = 60 * 1000; //1 мин анонс об окончании инстанса
                _timer.Elapsed += new ElapsedEventHandler(actionTimeUp);
                _timer.Enabled = true;
            }

            player.InstanceID = this.ServerID;

            _players.Add(player.ObjID, player);

            if (Template.WholeZoneIsPvp)
                player.setForcedPvpZone(true);

            if (Template.x0 != 0)
            {
                x = Template.x0;
                y = Template.y0;
                z = Template.z0;
            }
            else
            {
                x = player.X;
                y = player.Y;
                z = player.Z;
            }

            //debug
            player.sendMessage("you have entered "+Template.ClientName+" to "+TimeEnd.ToLocalTime());
        }

        private void actionTimeUp(object sender, ElapsedEventArgs e)
        {
            int seconds = (int)((TimeEnd.Ticks - DateTime.Now.Ticks) * 1E-7);
            if (seconds > 0)
            {
                //This dungeon will expire in $s1 minute(s). You will be forced out of the dungeon when the time expires.
                SystemMessage sm = new SystemMessage(2106);
                sm.addNumber(seconds/60);

                foreach (L2Player pl in _players.Values)
                    pl.sendPacket(sm);
            }
            else
            {
                close();
            }
        }

        public void join(L2Player player)
        {
            if (Template.joinFailed(player))
                return;

            player.InstanceID = this.ServerID;

            _players.Add(player.ObjID, player);

            if (Template.WholeZoneIsPvp)
                player.setForcedPvpZone(true);
        }

        public void close()
        {
            _timer.Enabled = false;
            foreach (L2Player pl in _players.Values)
            {
                pl.InstanceID = -1;
                pl.sendSystemMessage(2235);//The instance zone in use has been deleted and cannot be accessed.
                pl.teleport(x, y, z);

                if (Template.WholeZoneIsPvp)
                    pl.setForcedPvpZone(false);
            }
        }

        public bool startFailed(L2Player player) 
        {
            if (Template.startFailed(player))
                return true;

            return false; 
        }

        public bool joinFailed(L2Player player) 
        {
            if(Template.joinFailed(player))
                return true;

            if (Template.maxInside != -1)
            {
                return _players.Count >= Template.maxInside;
            }

            return false;
        }

        public bool closeFailed(L2Player player) 
        {
            if (Template.closeFailed(player))
                return true;

            return false; 
        }

        public void showLocation(L2Player player)
        {
            //Current location: $s1
            SystemMessage sm = new SystemMessage(2361); //todo на каждую по разному
            sm.addString(Template.ClientName);
            player.sendPacket(sm);
        }
    }
}
