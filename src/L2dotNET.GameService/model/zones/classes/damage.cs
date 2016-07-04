using System.Timers;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Zones.Classes
{
    class damage : L2Zone
    {
        public damage()
        {
            ZoneID = IdFactory.Instance.nextId();
        }

        public override void onInit()
        {
            _enabled = Template.DefaultStatus;

            if (_enabled && (Template._unit_tick > 0))
                startTimer();
        }

        public override void onTimerAction(object sender, ElapsedEventArgs e)
        {
            if (ObjectsInside.Count == 0)
                return;

            foreach (L2Object o in ObjectsInside.Values)
                if (o is L2Player)
                {
                    if (Template._target == ZoneTemplate.ZoneTarget.npc)
                        continue;

                    ((L2Player)o).ReduceHpArea(Template._damage_on_hp, Template._message_no);
                }
                else if (o is L2Warrior)
                {
                    if (Template._target == ZoneTemplate.ZoneTarget.pc)
                        continue;

                    ((L2Warrior)o).ReduceHpArea(Template._damage_on_hp, Template._message_no);
                }
        }

        public override void onEnter(L2Object obj)
        {
            if (!_enabled)
                return;

            base.onEnter(obj);

            obj.OnEnterZone(this);

            if (obj is L2Player)
            {
                L2Player p = (L2Player)obj;
                p.IsInDanger = true;
                p.SendPacket(new EtcStatusUpdate(p));
            }
        }

        public override void onExit(L2Object obj, bool cls)
        {
            if (!_enabled)
                return;

            base.onExit(obj, cls);

            obj.OnExitZone(this, cls);
            if (obj is L2Player)
            {
                L2Player p = (L2Player)obj;
                p.IsInDanger = false;
                p.SendPacket(new EtcStatusUpdate(p));
            }
        }
    }
}