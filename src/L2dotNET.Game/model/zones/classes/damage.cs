using L2dotNET.Game.tables;
using System.Timers;
using L2dotNET.Game.world;
using L2dotNET.Game.model.npcs;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.model.zones.classes
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

            if (_enabled && Template._unit_tick > 0)
                startTimer();
        }

        public override void onTimerAction(object sender, ElapsedEventArgs e)
        {
            if (ObjectsInside.Count == 0)
                return;

            foreach (L2Object o in ObjectsInside.Values)
            {
                if (o is L2Player)
                {
                    if (Template._target == ZoneTemplate.ZoneTarget.npc)
                        continue;

                    ((L2Player)o).reduceHpArea(Template._damage_on_hp, Template._message_no);
                }
                else if (o is L2Warrior)
                {
                    if (Template._target == ZoneTemplate.ZoneTarget.pc)
                        continue;

                    ((L2Warrior)o).reduceHpArea(Template._damage_on_hp, Template._message_no);
                }
            }
        }

        public override void onEnter(world.L2Object obj)
        {
            if (!_enabled)
                return;

            base.onEnter(obj);

            obj.onEnterZone(this);

            if (obj is L2Player)
            {
                L2Player p = (L2Player)obj;
                p.isInDanger = true;
                p.sendPacket(new EtcStatusUpdate(p));
            }
        }

        public override void onExit(world.L2Object obj, bool cls)
        {
            if (!_enabled)
                return;

            base.onExit(obj, cls);

            obj.onExitZone(this, cls);
            if (obj is L2Player)
            {
                L2Player p = (L2Player)obj;
                p.isInDanger = false;
                p.sendPacket(new EtcStatusUpdate(p));
            }
        }
    }
}
