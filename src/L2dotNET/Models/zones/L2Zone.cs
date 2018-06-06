using System.Collections.Generic;
using System.Timers;
using L2dotNET.Models.Player;
using L2dotNET.Models.Zones.forms;
using L2dotNET.Network;
using L2dotNET.Tables;
using L2dotNET.World;

namespace L2dotNET.Models.Zones
{
    public class L2Zone
    {
        public ZoneForm Territory;
        public int ZoneId;
        public string ZonePch;
        public bool Enabled = false;
        public ZoneTemplate Template;
        public int InstanceId = -1;
        public L2Object NpcCenter;

        protected readonly IdFactory _idFactory;

        public SortedList<int, L2Object> ObjectsInside = new SortedList<int, L2Object>();

        public virtual void OnEnter(L2Object obj)
        {
            if (!ObjectsInside.ContainsKey(obj.ObjId))
                ObjectsInside.Add(obj.ObjId, obj);
        }

        public void BroadcastPacket(GameserverPacket pk)
        {
            foreach (L2Object obj in ObjectsInside.Values)
            {
                if (obj is L2Player)
                    ((L2Player)obj).SendPacketAsync(pk);
                else
                {
                   
                }
            }
        }

        public virtual void OnExit(L2Object obj, bool cls)
        {
            if (!cls)
                return;

            lock (ObjectsInside)
            {
                if (ObjectsInside.ContainsKey(obj.ObjId))
                    ObjectsInside.Remove(obj.ObjId);
            }
        }

        public virtual void OnDie(L2Character obj, L2Character killer) { }

        public virtual void OnKill(L2Character obj, L2Character target) { }

        public Timer Action;

        public virtual void StartTimer()
        {
            Action = new Timer(Template.UnitTick * 1000);
            Action.Elapsed += OnTimerAction;
            Action.Interval = Template.UnitTick * 1000;
            Action.Enabled = true;
        }

        public virtual void StopTimer() { }

        public virtual void OnTimerAction(object sender, ElapsedEventArgs e) { }

        public virtual void OnInit() { }

        private Timer _selfDestruct;
        public int[] CylinderCenter;
        public string Name;

        protected L2Zone(IdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public void SelfDestruct(int sec)
        {
            _selfDestruct = new Timer(sec * 1000);
            _selfDestruct.Elapsed += DesctructTime;
            _selfDestruct.Enabled = true;
        }

        private void DesctructTime(object sender, ElapsedEventArgs e)
        {
            _selfDestruct.Enabled = false;

            NpcCenter.DecayMe();

            foreach (L2Object o in ObjectsInside.Values)
                OnExit(o, false);

            ObjectsInside.Clear();

            L2WorldRegion region = L2World.Instance.GetRegion(CylinderCenter[0], CylinderCenter[1]);
            if (region != null)
            {
                // region._zoneManager.remZone(this);
            }
        }
    }
}