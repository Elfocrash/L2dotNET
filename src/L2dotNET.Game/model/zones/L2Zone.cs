using System.Collections.Generic;
using System.Timers;
using L2dotNET.GameService.Model.playable;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.Model.zones.forms;
using L2dotNET.GameService.network;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.Model.zones
{
    public class L2Zone
    {
        public ZoneForm Territory;
        public int ZoneID;
        public string _zonePch;
        public bool _enabled = false;
        public ZoneTemplate Template;
        public int InstanceID = -1;
        public world.L2Object NpcCenter;

        public SortedList<int, L2Object> ObjectsInside = new SortedList<int, L2Object>();

        public virtual void onEnter(L2Object obj)
        {
            if (!ObjectsInside.ContainsKey(obj.ObjID))
                ObjectsInside.Add(obj.ObjID, obj);
        }

        public void broadcastPacket(GameServerNetworkPacket pk)
        {
            foreach (L2Object obj in ObjectsInside.Values)
                if (obj is L2Player)
                    obj.sendPacket(pk);
                else if (obj is L2Summon)
                    ((L2Summon)obj).sendPacket(pk);
        }

        public virtual void onExit(L2Object obj, bool cls)
        {
            if (cls)
            {
                lock (ObjectsInside)
                {
                    if (ObjectsInside.ContainsKey(obj.ObjID))
                        ObjectsInside.Remove(obj.ObjID);
                }
            }
        }

        public virtual void onDie(L2Character obj, L2Character killer) { }

        public virtual void onKill(L2Character obj, L2Character target) { }

        public System.Timers.Timer _action;

        public virtual void startTimer()
        {
            _action = new System.Timers.Timer(Template._unit_tick * 1000);
            _action.Elapsed += new ElapsedEventHandler(onTimerAction);
            _action.Interval = Template._unit_tick * 1000;
            _action.Enabled = true;
        }

        public virtual void stopTimer() { }

        public virtual void onTimerAction(object sender, ElapsedEventArgs e) { }

        public virtual void onInit() { }

        private Timer _selfDestruct;
        public int[] CylinderCenter;
        public string Name;

        public void SelfDestruct(int sec)
        {
            _selfDestruct = new Timer(sec * 1000);
            _selfDestruct.Elapsed += new ElapsedEventHandler(desctructTime);
            _selfDestruct.Enabled = true;
        }

        private void desctructTime(object sender, ElapsedEventArgs e)
        {
            _selfDestruct.Enabled = false;

            NpcCenter.deleteMe();

            foreach (L2Object o in ObjectsInside.Values)
            {
                onExit(o, false);
            }

            ObjectsInside.Clear();

            L2WorldRegion region = L2World.Instance.GetRegion(CylinderCenter[0], CylinderCenter[1]);
            if (region != null)
            {
                // region._zoneManager.remZone(this);
            }
        }
    }
}